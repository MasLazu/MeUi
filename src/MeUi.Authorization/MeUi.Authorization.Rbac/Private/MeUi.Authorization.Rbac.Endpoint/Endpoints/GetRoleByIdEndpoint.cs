using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Rbac.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.ApplicationContract.Dtos;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class GetRoleByIdEndpoint : EndointWithAuthorization<GetRoleByIdEndpoint, GetRoleByIdQuery, SuccessResponse<RoleDto>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Get("/v1/role/{id}");
        Description(x => x.WithTags("Role"));
        Summary(s =>
        {
            s.Summary = "Retrieve role details by ID";
            s.Description = "Returns detailed information about a specific role based on its unique identifier.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "ROLE";

    public override async Task HandleAsync(GetRoleByIdQuery req, CancellationToken ct)
    {
        RoleDto role = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<RoleDto>(
            role,
            "Role retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}