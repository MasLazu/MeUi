using System.Net;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class UpdateUserEndpoint : EndointWithAuthorization<UpdateUserEndpoint, UpdateUserCommand, SuccessResponse<Guid>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Put("/v1/users");
        Description(x => x.WithTags("User"));
        Summary(s =>
        {
            s.Summary = "Update an existing user";
            s.Description = "Updates the details of an existing user based on the provided information and returns the user's ID.";
        });
    }

    public static string Action() => "UPDATE";
    public static string Resource() => "USER";

    public override async Task HandleAsync(UpdateUserCommand req, CancellationToken ct)
    {
        Guid userId = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<Guid>(
            userId,
            "User updated successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}