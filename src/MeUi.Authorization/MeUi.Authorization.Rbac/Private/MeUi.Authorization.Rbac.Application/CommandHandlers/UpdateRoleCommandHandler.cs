using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authorization.Rbac.Application.CommandHandlers;

public class UpdateRoleCommandHandler : BaseCommandHandler<UpdateRoleCommand, Guid>
{
    private readonly IAuthorizationRbacRepository<Role> _roleRepository;

    public UpdateRoleCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationRbacRepository<Role> roleRepository) : base(unitOfWork)
    {
        _roleRepository = roleRepository;
    }

    public override async Task<Guid> ExecuteAsync(UpdateRoleCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            Role? role = await _roleRepository.FirstOrDefaultAsync(new RoleByIdSPesification(command.Id), ct) ??
                throw new NotFoundException("Role not found");

            role.Code = command.Code ?? role.Code;
            role.Name = command.Name ?? role.Name;
            role.Description = command.Description ?? role.Description;

            _roleRepository.Update(role, ct);
            return role.Id;
        }, ct, _roleRepository);
    }
}