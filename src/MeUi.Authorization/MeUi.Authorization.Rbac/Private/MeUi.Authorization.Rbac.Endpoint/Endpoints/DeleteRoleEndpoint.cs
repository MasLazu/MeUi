using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class DeleteRoleEndpoint : EndointWithAuthorization<DeleteRoleEndpoint, DeleteRoleCommand, SuccessResponse<Guid>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Delete("/v1/role/{id}");
        Description(x => x.WithTags("Role"));
        Summary(s =>
        {
            s.Summary = "Delete an existing role";
            s.Description = "Deletes a role identified by its unique ID from the RBAC system.";
        });
    }

    public static string Action() => "DELETE";
    public static string Resource() => "ROLE";

    public override async Task HandleAsync(DeleteRoleCommand req, CancellationToken ct)
    {
        Guid roleId = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<Guid>(
            roleId,
            "Role deleted successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}