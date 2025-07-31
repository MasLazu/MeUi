using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class PutUserRolesEndpoint : EndointWithAuthorization<PutUserRolesEndpoint, PutUserRolesCommand, SuccessResponse<IEnumerable<Guid>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Put("/v1/users/{id}/roles");
        Description(x => x.WithTags("UserRole"));
        Summary(s =>
        {
            s.Summary = "Update roles assigned to a user";
            s.Description = "This endpoint updates the roles assigned to a specific user. It creates new assignments and removes any that are no longer present in the request.";
        });
    }

    public static string Action() => "UPDATE";
    public static string Resource() => "USER_ROLE";

    public override async Task HandleAsync(PutUserRolesCommand req, CancellationToken ct)
    {
        IEnumerable<Guid> resourceActionIds = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<IEnumerable<Guid>>(
            resourceActionIds,
            "User roles updated successfully.",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}