using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateRefreshTokenCommandHandler : BaseCommandHandler<CreateRefreshTokenCommand, Guid>
{
    private readonly IAuthenticationCoreRepository<RefreshToken> _refreshRepository;

    public CreateRefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<RefreshToken> refreshRepository) : base(unitOfWork)
    {
        _refreshRepository = refreshRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateRefreshTokenCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync((ct) =>
        {
            var refreshToken = new RefreshToken()
            {
                Token = command.Token,
                ExpiresAt = command.ExpiresAt,
                RevokedAt = command.RevokedAt,
                UserId = command.UserId,
            };

            _refreshRepository.Add(refreshToken, ct);

            return refreshToken.Id;
        }, ct, _refreshRepository);
    }
}