using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Endpoint.Responses;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class TokenRefreshEndpoint : BaseEndpoint<TokenRefreshCommand, TokenRefreshResponse>
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/auth/token-refresh");
        AllowAnonymous();
        Description(x => x.WithTags("Auth"));
        Summary(s =>
        {
            s.Summary = "Refresh the access token using a valid refresh token.";
            s.Description = "Issues a new access token and sets a new refresh token as an HTTP-only cookie.";
        });
    }

    public override async Task HandleAsync(TokenRefreshCommand req, CancellationToken ct)
    {
        TokenRefreshResult result = await req.ExecuteAsync(ct);

        HttpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.RefreshTokenExpiresAt.DateTime
        });

        var response = new TokenRefreshResponse(
            new TokenRefreshResponseData()
            {
                AccessToken = result.AccessToken,
                AccessTokenExpiresAt = result.AccessTokenExpiresAt,
            }
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}