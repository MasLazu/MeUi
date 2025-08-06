using Mapster;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetUserPermissions;

public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<Domain.Entities.Action> _actionRepository;

    public GetUserPermissionsQueryHandler(
        IRepository<User> userRepository,
        IRepository<UserRole> userRoleRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IRepository<Permission> permissionRepository,
        IRepository<Resource> resourceRepository,
        IRepository<Domain.Entities.Action> actionRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
        _resourceRepository = resourceRepository;
        _actionRepository = actionRepository;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetUserPermissionsQuery request, CancellationToken ct)
    {
        // Get user
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
        {
            return new List<PermissionDto>();
        }

        // Get user roles
        var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == request.UserId, ct);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        if (!roleIds.Any())
        {
            return new List<PermissionDto>();
        }

        // Get role permissions
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => roleIds.Contains(rp.RoleId), ct);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).Distinct().ToList();

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