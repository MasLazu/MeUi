using System.Net;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class DeleteUserEndpoint : EndointWithAuthorization<DeleteUserEndpoint, DeleteUserCommand, SuccessResponse<Guid>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Delete("/v1/users/{id}");
        Description(x => x.WithTags("User"));
        Summary(s =>
        {
            s.Summary = "Delete a user";
            s.Description = "Deletes a user by their unique identifier and returns the ID of the deleted user.";
        });
    }

    public static string Action() => "DELETE";
    public static string Resource() => "USER";

    public override async Task HandleAsync(DeleteUserCommand req, CancellationToken ct)
    {
        Guid userId = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<Guid>(
            userId,
            "User deleted successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}