using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Queries.GetPageGroups;
using MeUi.Application.Features.Authorization.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetPageGroupsEndpoint : BaseEndpointWithoutRequest<IEnumerable<PageGroupDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/page-groups");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Get all page groups")
            .WithDescription("Retrieves all page groups with their associated pages"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetPageGroupsQuery();
        var result = await Mediator.Send(query, ct);
        await SendSuccessAsync(result, ct);
    }
}