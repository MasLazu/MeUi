using System.Net;
using MeUi.Shared.Endpoint.Responses;

namespace MeUi.Authentication.Password.Endpoints.Responses;

public class LoginResponse : SuccessResponse<LoginResponseData>
{
    public LoginResponse(LoginResponseData data) : base(data, "User logged in successfully", HttpStatusCode.OK)
    {
    }
}

public class LoginResponseData
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
}