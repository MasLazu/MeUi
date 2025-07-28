using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class TokenRefreshCommand : BaseCommand<TokenRefresh>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class TokenRefresh
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}