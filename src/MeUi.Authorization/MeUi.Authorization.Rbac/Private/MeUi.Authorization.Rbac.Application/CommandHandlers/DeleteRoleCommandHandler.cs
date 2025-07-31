using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authorization.Rbac.Application.CommandHandlers;

public class DeleteRoleCommandHandler : BaseCommandHandler<DeleteRoleCommand, Guid>
{
    private readonly IAuthorizationRbacRepository<Role> _roleRepository;
    private readonly IAuthorizationRbacRepository<RoleResourceAction> _roleResourceActionRepository;

    public DeleteRoleCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationRbacRepository<Role> roleRepository,
        IAuthorizationRbacRepository<RoleResourceAction> roleResourceActionRepository) : base(unitOfWork)
    {
        _roleRepository = roleRepository;
        _roleResourceActionRepository = roleResourceActionRepository;
    }

    public override async Task<Guid> ExecuteAsync(DeleteRoleCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            Role? role = await _roleRepository.FirstOrDefaultAsync(new RoleByIdSPesification(command.Id), ct) ??
                throw new NotFoundException("Role not found");
            List<RoleResourceAction> roleResourceActions = await _roleResourceActionRepository.ListAsync(new RoleResourceActionByRoleIdSpecification(command.Id), ct);

            _roleRepository.Delete(role, ct);
            _roleResourceActionRepository.DeleteRange(roleResourceActions, ct);

            return role.Id;
        }, ct, _roleRepository);
    }
}