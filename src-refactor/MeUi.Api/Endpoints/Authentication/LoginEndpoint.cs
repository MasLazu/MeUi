using MeUi.Api.Common;
using MeUi.Application.Features.Authentication.Commands.Login;
using MeUi.Application.Features.Authentication.Models;

namespace MeUi.Api.Endpoints.Authentication;

public class LoginEndpoint : BaseEndpoint<LoginRequest, TokenResponse>
{
    protected override void ConfigureEndpoint()
    {
        Post("/auth/login");
        AllowAnonymous();
        Description(x => x
            .WithTags("Authentication")
            .WithSummary("User login")
            .WithDescription("Authenticates a user and returns access and refresh tokens"));
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var command = new LoginCommand
        {
            EmailOrUsername = req.EmailOrUsername,
            Password = req.Password
        };

        var tokenResponse = await Mediator.Send(command, ct);
        await SendSuccessAsync(tokenResponse, ct);
    }
}