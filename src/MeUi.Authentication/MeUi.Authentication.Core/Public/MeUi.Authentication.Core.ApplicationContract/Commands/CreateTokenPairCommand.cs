using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class CreateTokenPairCommand : BaseCommand<CreateTokenPairResult>
{
    public Guid UserId { get; set; }
}

public class CreateTokenPairResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}