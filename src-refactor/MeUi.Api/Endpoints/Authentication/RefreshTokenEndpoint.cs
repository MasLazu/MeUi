using MeUi.Api.Common;
using MeUi.Application.Features.Authentication.Commands.RefreshToken;
using MeUi.Application.Features.Authentication.Models;

namespace MeUi.Api.Endpoints.Authentication;

public class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}

public class RefreshTokenEndpoint : BaseEndpoint<RefreshTokenRequest, TokenResponse>
{
    protected override void ConfigureEndpoint()
    {
        Post("/auth/refresh");
        AllowAnonymous();
        Description(x => x
            .WithTags("Authentication")
            .WithSummary("Refresh access token")
            .WithDescription("Refreshes an access token using a valid refresh token"));
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var command = new RefreshTokenCommand
        {
            RefreshToken = req.RefreshToken
        };

        var tokenResponse = await Mediator.Send(command, ct);
        await SendSuccessAsync(tokenResponse, ct);
    }
}