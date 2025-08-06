using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;


namespace MeUi.Application.Features.Authorization.Commands.AssignUserRoles;

public class AssignUserRolesCommandHandler : IRequestHandler<AssignUserRolesCommand, IEnumerable<Guid>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignUserRolesCommandHandler(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IRepository<UserRole> userRoleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Guid>> Handle(AssignUserRolesCommand request, CancellationToken ct)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{request.UserId}' not found");
        }

        // Verify all roles exist
        var existingRoles = new List<Guid>();
        foreach (var roleId in request.RoleIds)
        {
            var role = await _roleRepository.GetByIdAsync(roleId, ct);
            if (role != null)
            {
                existingRoles.Add(role.Id);
            }
        }

        var missingRoles = request.RoleIds.Except(existingRoles).ToList();
        if (missingRoles.Any())
        {
            throw new InvalidOperationException($"Roles not found: {string.Join(", ", missingRoles)}");
        }

        // Remove existing user roles
        var existingUserRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == request.UserId, ct);

        await _userRoleRepository.DeleteRangeAsync(existingUserRoles, ct);

        // Add new user roles
        foreach (var roleId in request.RoleIds)
        {
            var userRole = new UserRole
            {
                UserId = request.UserId,
                RoleId = roleId
            };
            await _userRoleRepository.AddAsync(userRole, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return request.RoleIds;
    }
}