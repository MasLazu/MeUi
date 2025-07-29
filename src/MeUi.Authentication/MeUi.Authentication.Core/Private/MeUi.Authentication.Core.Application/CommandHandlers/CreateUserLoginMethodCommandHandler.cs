using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateUserLoginMethodCommandHandler : BaseCommandHandler<CreateUserLoginMethodCommand, Guid>
{
    private readonly IAuthenticationCoreRepository<UserLoginMethod> _userLoginMethodRepository;

    public CreateUserLoginMethodCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<UserLoginMethod> userLoginMethodRepository) : base(unitOfWork)
    {
        _userLoginMethodRepository = userLoginMethodRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateUserLoginMethodCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync((ct) =>
        {
            var userLoginMethod = new UserLoginMethod()
            {
                UserId = command.UserId,
                LoginMethodCode = command.LoginMethodCode,
            };

            _userLoginMethodRepository.Add(userLoginMethod, ct);

            return userLoginMethod.Id;
        }, ct, _userLoginMethodRepository);
    }
}