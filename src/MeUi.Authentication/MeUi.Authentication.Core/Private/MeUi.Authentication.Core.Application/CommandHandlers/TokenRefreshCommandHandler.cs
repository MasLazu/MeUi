using FastEndpoints;
using MeUi.Authentication.Core.Application.Exceptions;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class TokenRefreshCommandHandler : BaseCommandHandler<TokenRefreshCommand, TokenRefreshResult>
{
    private readonly IAuthenticationCoreRepository<RefreshToken> _refreshTokenRepository;

    public TokenRefreshCommandHandler(IAuthenticationCoreRepository<RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public override async Task<TokenRefreshResult> ExecuteAsync(TokenRefreshCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByTokenSpesification(command.RefreshToken), ct);
            if (refreshToken == null || refreshToken.RevokedAt != null)
            {
                throw new InvalidRefreshTokenException();
            }

            refreshToken.RevokedAt = DateTime.Now;
            await _refreshTokenRepository.UpdateAsync(refreshToken, ct);

            var keyPair = await new CreateTokenPairCommand()
            {
                Transaction = Transaction,
                UserId = refreshToken.UserId
            }.ExecuteAsync(ct);

            return new TokenRefreshResult()
            {
                AccessToken = keyPair.AccessToken,
                AccessTokenExpiresAt = keyPair.AccessTokenExpiresAt,
                RefreshToken = keyPair.RefreshToken,
                RefreshTokenExpiresAt = keyPair.RefreshTokenExpiresAt,
            };
        }, command, ct, _refreshTokenRepository);
    }
}