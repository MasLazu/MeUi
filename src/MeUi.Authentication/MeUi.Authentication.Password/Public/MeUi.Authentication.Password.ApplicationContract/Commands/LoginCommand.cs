using FastEndpoints;
using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Password.ApplicationContract.Commands;

public class LoginCommand : BaseCommand<LoginResult>
{
    public string Identifier { get; set; }
    public string Password { get; set; }
}

public class LoginResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}