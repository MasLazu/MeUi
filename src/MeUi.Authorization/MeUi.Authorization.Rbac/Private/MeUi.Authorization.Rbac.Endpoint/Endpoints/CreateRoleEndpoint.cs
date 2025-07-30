using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class CreateRoleEndpoint : EndointWithAuthorization<CreateRoleEndpoint, CreateRoleCommand, SuccessResponse<Guid>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/role");
        Description(x => x.WithTags("Role"));
        Summary(s =>
        {
            s.Summary = "Create a new role";
            s.Description = "This endpoint allows the creation of a new role in the RBAC system.";
        });
    }

    public static string Action() => "CREATE";
    public static string Resource() => "ROLE";

    public override async Task HandleAsync(CreateRoleCommand req, CancellationToken ct)
    {
        Guid roleId = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<Guid>(
            roleId,
            "Role created successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}