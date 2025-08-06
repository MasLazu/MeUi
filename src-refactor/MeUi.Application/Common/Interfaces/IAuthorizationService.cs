namespace MeUi.Application.Common.Interfaces;

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(string permission, CancellationToken ct = default);
    Task<bool> HasAllPermissionsAsync(IEnumerable<string> permissions, CancellationToken ct = default);
    Task<bool> HasAnyPermissionAsync(IEnumerable<string> permissions, CancellationToken ct = default);
    Task<bool> HasRoleAsync(string role, CancellationToken ct = default);
    Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
}