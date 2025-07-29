using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Password.ApplicationContract.Commands;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.ApplicationContract.Commands;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Password.Application.Exceptions;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Password.Application.Constants;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Application.Interfaces;

namespace MeUi.Authentication.Password.Application.CommandHandlers;

public class RegisterCommandHandler : BaseCommandHandler<RegisterCommand, EmptyResult>
{
    private readonly IAuthenticationPasswordRepository<Domain.Entities.Password> _passwordRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationPasswordRepository<Domain.Entities.Password> passwordRepository,
        IPasswordHasher passwordHasher) : base(unitOfWork)
    {
        _passwordRepository = passwordRepository;
        _passwordHasher = passwordHasher;
    }

    public override async Task<EmptyResult> ExecuteAsync(RegisterCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            UserDto? user = await new GetUserByEmailQuery() { Email = command.Email }.ExecuteAsync(ct);
            if (user != null)
            {
                throw new UserAlreadyExistException(nameof(user.Email));
            }

            user = await new GetUserByUsernameQuery() { Username = command.Username }.ExecuteAsync(ct);
            if (user != null)
            {
                throw new UserAlreadyExistException(nameof(user.Username));
            }

            var createUserCommand = new CreateUserCommand()
            {
                Username = command.Username,
                Email = command.Email,
                Name = command.Name,
            };
            Guid userId = await createUserCommand.ExecuteAsync(ct);

            Guid userLoginMethodId = await new CreateUserLoginMethodCommand()
            { UserId = userId, LoginMethodCode = ApplicationConstant.PasswordLoginMethodCode }.ExecuteAsync(ct);

            var password = new Domain.Entities.Password()
            {
                UserLoginMethodId = userLoginMethodId,
                PasswordHash = _passwordHasher.HashPassword(command.Password),
            };
            _passwordRepository.Add(password, ct);

            return new EmptyResult();
        }, ct, _passwordRepository);
    }
}