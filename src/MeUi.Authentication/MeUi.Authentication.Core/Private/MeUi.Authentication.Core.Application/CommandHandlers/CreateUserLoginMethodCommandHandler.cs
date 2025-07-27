using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateUserLoginMethodCommandHandler : BaseCommandHandler<CreateUserLoginMethodCommand, Guid>
{
    private readonly IRepository<UserLoginMethod> _userLoginMethodRepository;

    public CreateUserLoginMethodCommandHandler(IRepository<UserLoginMethod> userLoginMethodRepository)
    {
        _userLoginMethodRepository = userLoginMethodRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateUserLoginMethodCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var userLoginMethod = new UserLoginMethod()
            {
                UserId = command.UserId,
                LoginMethodCode = command.LoginMethodCode,
            };

            await _userLoginMethodRepository.AddAsync(userLoginMethod, ct);

            return userLoginMethod.Id;
        }, command, ct, _userLoginMethodRepository);
    }
}