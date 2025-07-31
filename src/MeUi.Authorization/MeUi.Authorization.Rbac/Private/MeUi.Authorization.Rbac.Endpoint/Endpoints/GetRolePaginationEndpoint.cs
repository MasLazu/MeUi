using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Rbac.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.ApplicationContract.Dtos;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authorization.Rbac.Endpoint.Endpoints;

public class GetRolePaginationEndpoint : EndointWithAuthorization<GetRolePaginationEndpoint, GetRolesPaginationQuery, SuccessResponse<BasePaginationQueryResult<RoleDto>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/roles/pagination");
        Description(x => x.WithTags("Role"));
        Summary(s =>
        {
            s.Summary = "Get paginated list of roles";
            s.Description = "Retrieves a list of roles using pagination, filtering, and sorting.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "ROLE";

    public override async Task HandleAsync(GetRolesPaginationQuery req, CancellationToken ct)
    {
        BasePaginationQueryResult<RoleDto> role = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<BasePaginationQueryResult<RoleDto>>(
            role,
            "Successfully retrieved paginated role data.",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}