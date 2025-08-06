using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;


namespace MeUi.Application.Features.Authorization.Commands.AssignRolePermissions;

public class AssignRolePermissionsCommandHandler : IRequestHandler<AssignRolePermissionsCommand, IEnumerable<Guid>>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignRolePermissionsCommandHandler(
        IRepository<Role> roleRepository,
        IRepository<Permission> permissionRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Guid>> Handle(AssignRolePermissionsCommand request, CancellationToken ct)
    {
        // Verify role exists
        var role = await _roleRepository.GetByIdAsync(request.RoleId, ct);
        if (role == null)
        {
            throw new InvalidOperationException($"Role with ID '{request.RoleId}' not found");
        }

        // Verify all permissions exist
        var existingPermissions = new List<Guid>();
        foreach (var permissionId in request.PermissionIds)
        {
            var permission = await _permissionRepository.GetByIdAsync(permissionId, ct);
            if (permission != null)
            {
                existingPermissions.Add(permission.Id);
            }
        }

        var missingPermissions = request.PermissionIds.Except(existingPermissions).ToList();
        if (missingPermissions.Any())
        {
            throw new InvalidOperationException($"Permissions not found: {string.Join(", ", missingPermissions)}");
        }

        // Remove existing role permissions
        var existingRolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == request.RoleId, ct);

        await _rolePermissionRepository.DeleteRangeAsync(existingRolePermissions, ct);

        // Add new role permissions
        foreach (var permissionId in request.PermissionIds)
        {
            var rolePermission = new RolePermission
            {
                RoleId = request.RoleId,
                PermissionId = permissionId
            };
            await _rolePermissionRepository.AddAsync(rolePermission, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return request.PermissionIds;
    }
}