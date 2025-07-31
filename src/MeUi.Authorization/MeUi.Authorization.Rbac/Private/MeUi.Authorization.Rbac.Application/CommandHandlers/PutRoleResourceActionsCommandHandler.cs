using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authorization.Rbac.Application.CommandHandlers;

public class PutRoleResourceActionsCommandHandler : BaseCommandHandler<PutRoleResourceActionsCommand, IEnumerable<Guid>>
{
    private readonly IAuthorizationRbacRepository<RoleResourceAction> _roleResourceActionRepository;

    public PutRoleResourceActionsCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationRbacRepository<RoleResourceAction> roleResourceActionRepository) : base(unitOfWork)
    {
        _roleResourceActionRepository = roleResourceActionRepository;
    }

    public override async Task<IEnumerable<Guid>> ExecuteAsync(PutRoleResourceActionsCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            List<RoleResourceAction> existingResourceActions = await _roleResourceActionRepository.ListAsync(new RoleResourceActionByRoleIdSpecification(command.RoleId), ct);
            IEnumerable<Guid> existingResourceActionIds = existingResourceActions.Select(era => era.Id);

            var resourceActionsToAdd = command.ResourceActionIds
                .Where(id => existingResourceActionIds.Contains(id))
                .Select(id => new RoleResourceAction()
                {
                    RoleId = command.RoleId,
                    ResourceActionId = id
                }).ToList();

            var resourceActionToDelete = existingResourceActions.Where(era => !command.ResourceActionIds.Contains(era.ResourceActionId)).ToList();

            _roleResourceActionRepository.AddRange(resourceActionsToAdd, ct);
            _roleResourceActionRepository.DeleteRange(resourceActionToDelete, ct);

            return command.ResourceActionIds;
        }, ct, _roleResourceActionRepository);
    }
}