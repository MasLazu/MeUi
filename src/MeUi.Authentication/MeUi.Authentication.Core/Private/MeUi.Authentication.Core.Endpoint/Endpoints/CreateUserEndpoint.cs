using System.Net;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class CreateUserEndpoint : EndointWithAuthorization<CreateUserEndpoint, CreateUserCommand, SuccessResponse<Guid>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/users");
        Description(x => x.WithTags("User"));
        Summary(s =>
        {
            s.Summary = "Create a new user";
            s.Description = "Creates a new user with the provided information and returns the ID of the created user.";
        });
    }

    public static string Action() => "CREATE";
    public static string Resource() => "USER";

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        Guid userId = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<Guid>(
            userId,
            "User created successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}