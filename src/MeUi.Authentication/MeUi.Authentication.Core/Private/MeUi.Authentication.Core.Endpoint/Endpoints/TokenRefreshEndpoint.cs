using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Endpoint.Responses;
using MeUi.Shared.Application.Exceptions;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class TokenRefreshEndpoint : BaseEndpointWithoutRequest<TokenRefreshResponse>
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

    public override async Task HandleAsync(CancellationToken ct)
    {
        string? refreshToken = HttpContext.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedException("Refresh token not found.");
        }

        TokenRefreshResult result = await new TokenRefreshCommand()
        {
            RefreshToken = refreshToken
        }.ExecuteAsync(ct);

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