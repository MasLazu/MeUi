using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class UpdateRoleEndpoint : EndointWithAuthorization<UpdateRoleEndpoint, UpdateRoleCommand, SuccessResponse<Guid>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Put("/v1/role");
        Description(x => x.WithTags("Role"));
        Summary(s =>
        {
            s.Summary = "Update an existing role";
            s.Description = "Updates the details of an existing role in the RBAC system using the provided data.";
        });
    }

    public static string Action() => "UPDATE";
    public static string Resource() => "ROLE";

    public override async Task HandleAsync(UpdateRoleCommand req, CancellationToken ct)
    {
        Guid roleId = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<Guid>(
            roleId,
            "Role updated successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}