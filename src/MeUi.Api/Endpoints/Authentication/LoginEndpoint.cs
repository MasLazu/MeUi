using MeUi.Api.Endpoints;
using MeUi.Application.Features.Authentication.Commands.Login;
using MeUi.Application.Features.Authentication.Models;

namespace MeUi.Api.Endpoints.Authentication;

public class LoginEndpoint : BaseEndpoint<LoginCommand, TokenResponse>
{
    public override void ConfigureEndpoint()
    {
        Post("/auth/login");
        AllowAnonymous();
        Description(x => x.WithTags("Authentication").WithSummary("User login"));
    }

    public override async Task HandleAsync(LoginCommand req, CancellationToken ct)
    {
        TokenResponse tokenResponse = await Mediator.Send(req, ct);
        await SendSuccessAsync(tokenResponse, "Login successful", ct);
    }
}