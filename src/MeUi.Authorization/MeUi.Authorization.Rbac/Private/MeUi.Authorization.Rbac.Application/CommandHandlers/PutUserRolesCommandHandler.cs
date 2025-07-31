using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authorization.Rbac.Application.CommandHandlers;

public class PutUserRolesCommandHandler : BaseCommandHandler<PutUserRolesCommand, IEnumerable<Guid>>
{
    private readonly IAuthorizationRbacRepository<UserRole> _userRoleRepository;
    private readonly IAuthorizationRbacRepository<Role> _roleRepository;

    public PutUserRolesCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationRbacRepository<UserRole> userRoleRepository,
        IAuthorizationRbacRepository<Role> roleRepository) : base(unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
    }

    public override async Task<IEnumerable<Guid>> ExecuteAsync(PutUserRolesCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            UserDto? user = await new GetUserByIdQuery() { Id = command.UserId }.ExecuteAsync(ct);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            List<UserRole> existingUserRoles = await _userRoleRepository.ListAsync(new UserRoleByUserIdSpesification(command.UserId), ct);

            List<Role> roles = await _roleRepository.ListAsync(new RoleByIdsSpesification(command.RoleIds), ct);

            IEnumerable<UserRole> userRolesToAdd = roles.Select(r => new UserRole()
            {
                UserId = command.UserId,
                RoleId = r.Id
            }).Where(r => !existingUserRoles.Select(eur => eur.RoleId).Contains(r.Id));

            List<UserRole> userRolesToDelete = existingUserRoles.Where(
                ur => !command.RoleIds.Contains(ur.RoleId)
            ).ToList();

            _userRoleRepository.AddRange(userRolesToAdd, ct);
            _userRoleRepository.DeleteRange(userRolesToDelete, ct);

            return command.RoleIds;
        }, ct, _userRoleRepository);
    }
}