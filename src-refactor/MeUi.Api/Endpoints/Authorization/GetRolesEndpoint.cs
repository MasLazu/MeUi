using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Queries.GetRolesPaginated;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Application.Common.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetRolesRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}

public class GetRolesEndpoint : BaseEndpoint<GetRolesRequest, PaginatedResult<RoleDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/roles");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Get paginated list of roles")
            .WithDescription("Retrieves a paginated list of roles with optional search"));
    }

    public override async Task HandleAsync(GetRolesRequest req, CancellationToken ct)
    {
        var query = new GetRolesPaginatedQuery
        {
            Page = req.Page,
            PageSize = req.PageSize,
            Search = req.Search
        };

        var result = await Mediator.Send(query, ct);
        await SendSuccessAsync(result, ct);
    }
}