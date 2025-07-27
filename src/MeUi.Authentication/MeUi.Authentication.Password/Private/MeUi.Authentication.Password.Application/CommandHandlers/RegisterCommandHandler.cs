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

namespace MeUi.Authentication.Password.Application.CommandHandlers;

public class RegisterCommandHandler : BaseCommandHandler<RegisterCommand, EmptyResult>
{
    private readonly IRepository<Domain.Entities.Password> _passwordRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IRepository<Domain.Entities.Password> passwordRepository, IPasswordHasher passwordHasher)
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
                throw new UserAlreadyExistException(nameof(user.Name));
            }

            user = await new GetUserByUsernameQuery() { Username = command.Username }.ExecuteAsync(ct);
            if (user != null)
            {
                throw new UserAlreadyExistException(nameof(user.Name));
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
            await _passwordRepository.AddAsync(password, ct);

            return new EmptyResult();
        }, command, ct, _passwordRepository);
    }
}