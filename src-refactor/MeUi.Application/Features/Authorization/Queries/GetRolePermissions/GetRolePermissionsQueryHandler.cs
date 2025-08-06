using Mapster;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetRolePermissions;

public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<Domain.Entities.Action> _actionRepository;

    public GetRolePermissionsQueryHandler(
        IRepository<Role> roleRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IRepository<Permission> permissionRepository,
        IRepository<Resource> resourceRepository,
        IRepository<Domain.Entities.Action> actionRepository)
    {
        _roleRepository = roleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
        _resourceRepository = resourceRepository;
        _actionRepository = actionRepository;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetRolePermissionsQuery request, CancellationToken ct)
    {
        // Get role
        var role = await _roleRepository.GetByIdAsync(request.RoleId, ct);
        if (role == null)
        {
            return new List<PermissionDto>();
        }

        // Get role permissions
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == request.RoleId, ct);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

        if (!permissionIds.Any())
        {
            return new List<PermissionDto>();
        }

        // Get permissions
        var permissions = await _permissionRepository.FindAsync(p => permissionIds.Contains(p.Id), ct);

        // Get resources and actions for mapping
        var resourceCodes = permissions.Select(p => p.ResourceCode).Distinct().ToList();
        var actionCodes = permissions.Select(p => p.ActionCode).Distinct().ToList();

        var resources = await _resourceRepository.FindAsync(r => resourceCodes.Contains(r.Code), ct);
        var actions = await _actionRepository.FindAsync(a => actionCodes.Contains(a.Code), ct);

        // Map to DTOs
        var permissionDtos = permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            ResourceId = resources.FirstOrDefault(r => r.Code == p.ResourceCode)?.Id ?? Guid.Empty,
            ActionId = actions.FirstOrDefault(a => a.Code == p.ActionCode)?.Id ?? Guid.Empty,
            ResourceCode = p.ResourceCode,
            ActionCode = p.ActionCode,
            Resource = resources.FirstOrDefault(r => r.Code == p.ResourceCode)?.Adapt<ResourceDto>(),
            Action = actions.FirstOrDefault(a => a.Code == p.ActionCode)?.Adapt<ActionDto>(),
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });

        return permissionDtos;
    }
}