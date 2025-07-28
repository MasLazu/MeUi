using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class CreateRefreshTokenCommand : BaseCommand<Guid>
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid UserId { get; set; }
}