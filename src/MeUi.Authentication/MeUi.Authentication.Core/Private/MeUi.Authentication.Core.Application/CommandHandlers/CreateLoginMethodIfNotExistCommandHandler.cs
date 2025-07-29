using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateLoginMethodIfNotExistCommandHandler : BaseCommandHandler<CreateLoginMethodIfNotExistCommand, Guid>
{
    private readonly IAuthenticationCoreRepository<LoginMethod> _loginMethodRepository;

    public CreateLoginMethodIfNotExistCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<LoginMethod> loginMethodRepository) : base(unitOfWork)
    {
        _loginMethodRepository = loginMethodRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateLoginMethodIfNotExistCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            LoginMethod? loginMethod = await _loginMethodRepository.FirstOrDefaultAsync(new LoginMethodByCodeSepsification(command.Code), ct);

            if (loginMethod != null)
            {
                return loginMethod.Id;
            }

            loginMethod = new LoginMethod()
            {
                Code = command.Code,
                Name = command.Name,
                Description = command.Description,
            };

            _loginMethodRepository.Add(loginMethod, ct);

            return loginMethod.Id;
        }, ct, _loginMethodRepository);
    }
}