using MeUi.Application.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Infrastructure.Services.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<Role> _roleRepository;

    public AuthorizationService(
        ICurrentUser currentUser,
        IRepository<User> userRepository,
        IRepository<UserRole> userRoleRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IRepository<Permission> permissionRepository,
        IRepository<Role> roleRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
    }

    public async Task<bool> HasPermissionAsync(string permission, CancellationToken ct = default)
    {
        if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
        {
            return false;
        }

        // Check if user has the permission directly from current user context

        if (_currentUser.Permissions.Contains(permission))
        {

            return true;
        }

        // Fallback: Query database for fresh permissions

        var userPermissions = await GetUserPermissionsAsync(_currentUser.UserId.Value, ct);
        return userPermissions.Contains(permission);
    }

    public async Task<bool> HasAllPermissionsAsync(IEnumerable<string> permissions, CancellationToken ct = default)
    {
        if (!permissions.Any())
        {
            return true;
        }


        foreach (string permission in permissions)
        {
            if (!await HasPermissionAsync(permission, ct))
            {

                return false;
            }

        }
        return true;
    }

    public async Task<bool> HasAnyPermissionAsync(IEnumerable<string> permissions, CancellationToken ct = default)
    {

        if (!permissions.Any())
        {
            return true;
        }


        foreach (string permission in permissions)
        {
            if (await HasPermissionAsync(permission, ct))
            {

                return true;
            }

        }
        return false;
    }

    public async Task<bool> HasRoleAsync(string role, CancellationToken ct = default)
    {
        if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
        {
            return false;
        }

        // Check if user has the role directly from current user context

        if (_currentUser.Roles.Contains(role))
        {

            return true;
        }

        // Fallback: Query database for fresh roles

        var userRoles = await GetUserRolesAsync(_currentUser.UserId.Value, ct);
        return userRoles.Contains(role);
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default)
    {
        // Get user roles
        var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == userId, ct);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        if (!roleIds.Any())
        {

            return new List<string>();
        }

        // Get role permissions

        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => roleIds.Contains(rp.RoleId), ct);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).Distinct().ToList();

        if (!permissionIds.Any())
        {

            return new List<string>();
        }

        // Get permissions and format them as "action:resource"

        var permissions = await _permissionRepository.FindAsync(p => permissionIds.Contains(p.Id), ct);
        return permissions.Select(p => $"{p.ActionCode}:{p.ResourceCode}").ToList();
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
    {
        // Get user roles
        var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == userId, ct);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        if (!roleIds.Any())
        {

            return new List<string>();
        }

        // Get role details

        var roles = await _roleRepository.FindAsync(r => roleIds.Contains(r.Id), ct);
        return roles.Select(r => r.Code).ToList();
    }
}