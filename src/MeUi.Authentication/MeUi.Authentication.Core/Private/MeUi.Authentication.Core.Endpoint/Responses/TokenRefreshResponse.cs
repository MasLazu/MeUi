using MeUi.Shared.Endpoint.Responses;
using System.Net;

namespace MeUi.Authentication.Core.Endpoint.Responses;

public class TokenRefreshResponse : SuccessResponse<TokenRefreshResponseData>
{
    public TokenRefreshResponse(TokenRefreshResponseData data) : base(data, "Token refreshed successfully.", HttpStatusCode.OK)
    {
    }
}

public class TokenRefreshResponseData
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
}