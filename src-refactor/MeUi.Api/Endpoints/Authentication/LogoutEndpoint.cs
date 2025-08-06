using MeUi.Api.Common;
using MeUi.Application.Features.Authentication.Commands.Logout;

namespace MeUi.Api.Endpoints.Authentication;

public class LogoutRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}

public class LogoutEndpoint : BaseEndpoint<LogoutRequest, bool>
{
    protected override void ConfigureEndpoint()
    {
        Post("/auth/logout");
        AllowAnonymous(); // TODO: Should require authentication
        Description(x => x
            .WithTags("Authentication")
            .WithSummary("User logout")
            .WithDescription("Logs out a user by invalidating their refresh token"));
    }

    public override async Task HandleAsync(LogoutRequest req, CancellationToken ct)
    {
        var command = new LogoutCommand
        {
            RefreshToken = req.RefreshToken
        };

        var result = await Mediator.Send(command, ct);
        await SendSuccessAsync(result, ct);
    }
}