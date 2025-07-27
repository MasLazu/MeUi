using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateRefreshTokenCommandHandler : BaseCommandHandler<CreateRefreshTokenCommand, Guid>
{
    private readonly IRepository<RefreshToken> _refreshRepository;

    public CreateRefreshTokenCommandHandler(IRepository<RefreshToken> refreshRepository)
    {
        _refreshRepository = refreshRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateRefreshTokenCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var refreshToken = new RefreshToken()
            {
                Token = command.Token,
                ExpiresAt = command.ExpiresAt,
                RevokedAt = command.RevokedAt,
                UserId = command.UserId,
            };

            await _refreshRepository.AddAsync(refreshToken, ct);

            return refreshToken.Id;
        }, command, ct, _refreshRepository);
    }
}