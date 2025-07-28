using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class UpdateRefreshTokenCommandHandler : BaseCommandHandler<UpdateRefreshTokenCommand, Guid>
{
    private readonly IAuthenticationCoreRepository<RefreshToken> _refreshRepository;

    public UpdateRefreshTokenCommandHandler(IAuthenticationCoreRepository<RefreshToken> refreshRepository)
    {
        _refreshRepository = refreshRepository;
    }

    public override async Task<Guid> ExecuteAsync(UpdateRefreshTokenCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var spesification = new RefreshTokenByIdSpesification(command.Id);
            RefreshToken refreshToken = await _refreshRepository.FirstOrDefaultAsync(spesification, ct) ?? throw new NotFoundException(nameof(RefreshToken));

            refreshToken.Token = command.Token ?? refreshToken.Token;
            refreshToken.ExpiresAt = command.ExpiresAt ?? refreshToken.ExpiresAt;
            refreshToken.RevokedAt = command.RevokedAt ?? refreshToken.RevokedAt;
            refreshToken.UserId = command.UserId ?? refreshToken.UserId;

            await _refreshRepository.UpdateAsync(refreshToken, ct);

            return refreshToken.Id;
        }, command, ct, _refreshRepository);
    }
}