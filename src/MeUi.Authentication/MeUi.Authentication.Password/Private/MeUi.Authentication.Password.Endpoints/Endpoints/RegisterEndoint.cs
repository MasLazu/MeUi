using System.Net;
using FastEndpoints;
using MeUi.Authentication.Password.ApplicationContract.Commands;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;

namespace MeUi.Authentication.Password.Endpoints.Endpoints;

public class RegisterEndpoint : BaseEndpoint<RegisterCommand, SuccessResponse<object>>
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/auth/register");
        Summary(s =>
        {
            s.Summary = "Register new user";
            s.Description = "Register new user with login method password";
        });
    }

    public override async Task HandleAsync(RegisterCommand req, CancellationToken ct)
    {
        await req.ExecuteAsync(ct);

        var response = new SuccessResponse<object>(
            new object(),
            "User registered successfully",
            HttpStatusCode.Created
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.Created, ct);
    }
}