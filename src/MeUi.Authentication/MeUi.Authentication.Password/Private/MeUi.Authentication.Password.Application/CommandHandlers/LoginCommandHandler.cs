using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Password.ApplicationContract.Commands;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Password.Application.Exceptions;
using MeUi.Authentication.Password.Application.Constants;
using MeUi.Authentication.Password.Application.Spesifications;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Application.Interfaces;

namespace MeUi.Authentication.Password.Application.CommandHandlers;

public class LoginCommandHandler : BaseCommandHandler<LoginCommand, LoginResult>
{
    private readonly IAuthenticationPasswordRepository<Domain.Entities.Password> _passwordRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(IAuthenticationPasswordRepository<Domain.Entities.Password> passwordRepository, IPasswordHasher passwordHasher)
    {
        _passwordRepository = passwordRepository;
        _passwordHasher = passwordHasher;
    }


    public override async Task<LoginResult> ExecuteAsync(LoginCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            UserDto user = await new GetUserWithLoginMethodsByIdentifierQuery()
            { Identifier = command.Identifier }.ExecuteAsync(ct) ?? throw new InvalidCredentialException();

            if (user.IsSuspended)
            {
                throw new AccountSuspendedException();
            }

            UserLoginMethodDto passwordLoginMethod = user.LoginMethods
                .FirstOrDefault(lm => lm.LoginMethodCode == ApplicationConstant.PasswordLoginMethodCode)
                    ?? throw new InvalidCredentialException();

            var spesification = new PasswordByUserLoginMethodIdSpesification(passwordLoginMethod.Id);
            Domain.Entities.Password password = await _passwordRepository
                .FirstOrDefaultAsync(spesification, ct)
                    ?? throw new InvalidCredentialException();

            if (!_passwordHasher.VerifyPassword(command.Password, password.PasswordHash))
            {
                throw new InvalidCredentialException();
            }

            CreateTokenPairResult keyPair = await new CreateTokenPairCommand() { UserId = user.Id }.ExecuteAsync(ct);

            return new LoginResult()
            {
                AccessToken = keyPair.AccessToken,
                RefreshToken = keyPair.RefreshToken,
                AccessTokenExpiresAt = keyPair.AccessTokenExpiresAt,
                RefreshTokenExpiresAt = keyPair.RefreshTokenExpiresAt,
            };
        }, command, ct, _passwordRepository);
    }
}