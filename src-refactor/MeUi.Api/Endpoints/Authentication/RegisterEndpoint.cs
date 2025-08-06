using MeUi.Api.Common;
using MeUi.Application.Features.Authentication.Commands.Register;
using MeUi.Application.Features.Authentication.Models;

namespace MeUi.Api.Endpoints.Authentication;

public class RegisterEndpoint : BaseEndpoint<RegisterRequest, TokenResponse>
{
    protected override void ConfigureEndpoint()
    {
        Post("/auth/register");
        AllowAnonymous();
        Description(x => x
            .WithTags("Authentication")
            .WithSummary("User registration")
            .WithDescription("Registers a new user and returns access and refresh tokens"));
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var command = new RegisterCommand
        {
            Username = req.Username,
            Email = req.Email,
            Name = req.Name,
            Password = req.Password
        };

        var tokenResponse = await Mediator.Send(command, ct);
        await SendSuccessAsync(tokenResponse, ct);
    }
}