using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.CheckUserPermission;

public class CheckUserPermissionQueryHandler : IRequestHandler<CheckUserPermissionQuery, bool>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<Domain.Entities.Action> _actionRepository;

    public CheckUserPermissionQueryHandler(
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

    public async Task<bool> Handle(CheckUserPermissionQuery request, CancellationToken ct)
    {
        // Get user
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
        {
            return false;
        }

        // Get user roles
        var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == request.UserId, ct);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        if (!roleIds.Any())
        {
            return false;
        }

        // Get role permissions
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => roleIds.Contains(rp.RoleId), ct);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

        if (!permissionIds.Any())
        {
            return false;
        }

        // Get permissions
        var permissions = await _permissionRepository.FindAsync(p => permissionIds.Contains(p.Id), ct);

        // Get resources and actions
        var resources = await _resourceRepository.FindAsync(r => r.Code == request.ResourceCode, ct);
        var actions = await _actionRepository.FindAsync(a => a.Code == request.ActionCode, ct);

        var resource = resources.FirstOrDefault();
        var action = actions.FirstOrDefault();

        if (resource == null || action == null)
        {
            return false;
        }

        // Check if user has the required permission
        return permissions.Any(p => p.ResourceCode == request.ResourceCode && p.ActionCode == request.ActionCode);
    }
}