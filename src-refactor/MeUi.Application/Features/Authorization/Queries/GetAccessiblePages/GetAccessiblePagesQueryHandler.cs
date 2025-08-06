using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetAccessiblePages;

public class GetAccessiblePagesQueryHandler : IRequestHandler<GetAccessiblePagesQuery, IEnumerable<PageGroupDto>>
{
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<PageGroup> _pageGroupRepository;
    private readonly IRepository<Page> _pageRepository;
    private readonly IRepository<PageResourceAction> _pageResourceActionRepository;

    public GetAccessiblePagesQueryHandler(
        IRepository<UserRole> userRoleRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IRepository<Permission> permissionRepository,
        IRepository<PageGroup> pageGroupRepository,
        IRepository<Page> pageRepository,
        IRepository<PageResourceAction> pageResourceActionRepository)
    {
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
        _pageGroupRepository = pageGroupRepository;
        _pageRepository = pageRepository;
        _pageResourceActionRepository = pageResourceActionRepository;
    }

    public async Task<IEnumerable<PageGroupDto>> Handle(GetAccessiblePagesQuery request, CancellationToken cancellationToken)
    {
        // Get user roles
        var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == request.UserId, cancellationToken);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        if (!roleIds.Any())
        {
            return new List<PageGroupDto>();
        }

        // Get role permissions
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => roleIds.Contains(rp.RoleId), cancellationToken);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).Distinct().ToList();

        if (!permissionIds.Any())
        {
            return new List<PageGroupDto>();
        }

        // Get permissions
        var permissions = await _permissionRepository.FindAsync(p => permissionIds.Contains(p.Id), cancellationToken);

        // Get page resource actions that match user permissions
        var pageResourceActions = await _pageResourceActionRepository.FindAsync(pra => permissionIds.Contains(pra.PermissionId!.Value), cancellationToken);
        var accessiblePageIds = pageResourceActions.Select(pra => pra.PageId!.Value).Distinct().ToList();

        if (!accessiblePageIds.Any())
        {
            return new List<PageGroupDto>();
        }

        // Get accessible pages
        var accessiblePages = await _pageRepository.FindAsync(p => accessiblePageIds.Contains(p.Id), cancellationToken);
        var pageGroupIds = accessiblePages.Select(p => p.PageGroupId!.Value).Distinct().ToList();

        // Get page groups
        var pageGroups = await _pageGroupRepository.FindAsync(pg => pageGroupIds.Contains(pg.Id), cancellationToken);

        return pageGroups.Select(pg => new PageGroupDto
        {
            Id = pg.Id,
            Code = pg.Code,
            Name = pg.Name,
            Icon = pg.Icon,
            CreatedAt = pg.CreatedAt,
            UpdatedAt = pg.UpdatedAt,
            Pages = accessiblePages
                .Where(p => p.PageGroupId == pg.Id)
                .Select(p => new PageDto
                {
                    Id = p.Id,
                    ParentId = p.ParentId,
                    PageGroupId = p.PageGroupId,
                    Code = p.Code,
                    Name = p.Name,
                    Path = p.Path,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList()
        });
    }
}