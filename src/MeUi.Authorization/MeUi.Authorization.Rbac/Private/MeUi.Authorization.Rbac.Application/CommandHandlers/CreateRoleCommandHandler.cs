using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authorization.Rbac.Application.CommandHandlers;

public class CreateRoleCommandHandler : BaseCommandHandler<CreateRoleCommand, Guid>
{
    private readonly IAuthorizationRbacRepository<Role> _roleRepository;

    public CreateRoleCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationRbacRepository<Role> roleRepository) : base(unitOfWork)
    {
        _roleRepository = roleRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateRoleCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            Role? role = await _roleRepository.FirstOrDefaultAsync(new RoleByCodeSPesification(command.Code), ct);

            if (role != null)
            {
                throw new ConflictException("Role Code already exists");
            }

            role = new Role()
            {
                Code = command.Code,
                Name = command.Name,
                Description = command.Description,
            };

            _roleRepository.Add(role, ct);
            return role.Id;
        }, ct, _roleRepository);
    }
}