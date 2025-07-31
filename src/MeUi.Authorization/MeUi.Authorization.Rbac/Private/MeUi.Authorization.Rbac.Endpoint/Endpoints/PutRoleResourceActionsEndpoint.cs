using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class PutRoleResourceActionsEndpoint : EndointWithAuthorization<PutRoleResourceActionsEndpoint, PutRoleResourceActionsCommand, SuccessResponse<IEnumerable<Guid>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Put("/v1/roles/{id}/resource-actions");
        Description(x => x.WithTags("Role"));
        Summary(s =>
        {
            s.Summary = "Update resource actions for a role";
            s.Description = "Updates the list of resource actions assigned to a specific role by its ID.";
        });
    }

    public static string Action() => "UPDATE";
    public static string Resource() => "ROLE_RESOURCE_ACTION";

    public override async Task HandleAsync(PutRoleResourceActionsCommand req, CancellationToken ct)
    {
        IEnumerable<Guid> resourceActionIds = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<IEnumerable<Guid>>(
            resourceActionIds,
            "Successfully updated role's resource actions.",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}