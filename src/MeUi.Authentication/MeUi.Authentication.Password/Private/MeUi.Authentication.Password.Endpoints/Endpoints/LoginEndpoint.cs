using System.Net;
using FastEndpoints;
using MeUi.Authentication.Password.ApplicationContract.Commands;
using MeUi.Authentication.Password.Endpoints.Responses;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;

namespace MeUi.Authentication.Password.Endpoints.Endpoints;

public class LoginEndpoint : BaseEndpoint<LoginCommand, LoginResponse>
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/auth/login");
        Summary(s =>
        {
            s.Summary = "Login user";
            s.Description = "Login user with login method password";
        });
    }

    public override async Task HandleAsync(LoginCommand req, CancellationToken ct)
    {
        LoginResult result = await req.ExecuteAsync(ct);

        HttpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.RefreshTokenExpiresAt.DateTime
        });

        var response = new LoginResponse(
            new LoginResponseData()
            {
                AccessToken = result.AccessToken,
                AccessTokenExpiresAt = result.AccessTokenExpiresAt,
            }
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.Created, ct);
    }
}