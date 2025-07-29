using FastEndpoints;
using MeUi.Authentication.Core.Application.Exceptions;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class TokenRefreshCommandHandler : BaseCommandHandler<TokenRefreshCommand, TokenRefreshResult>
{
    private readonly IAuthenticationCoreRepository<RefreshToken> _refreshTokenRepository;

    public TokenRefreshCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<RefreshToken> refreshTokenRepository) : base(unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public override async Task<TokenRefreshResult> ExecuteAsync(TokenRefreshCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByTokenSpesification(command.RefreshToken), ct);
            if (refreshToken == null || refreshToken.RevokedAt != null)
            {
                throw new InvalidRefreshTokenException();
            }

            refreshToken.RevokedAt = DateTime.UtcNow;
            _refreshTokenRepository.Update(refreshToken, ct);

            CreateTokenPairResult keyPair = await new CreateTokenPairCommand()
            {
                UserId = refreshToken.UserId
            }.ExecuteAsync(ct);

            return new TokenRefreshResult()
            {
                AccessToken = keyPair.AccessToken,
                AccessTokenExpiresAt = keyPair.AccessTokenExpiresAt,
                RefreshToken = keyPair.RefreshToken,
                RefreshTokenExpiresAt = keyPair.RefreshTokenExpiresAt,
            };
        }, ct, _refreshTokenRepository);
    }
}