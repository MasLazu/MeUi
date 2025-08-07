using MeUi.Api.Endpoints;
using MeUi.Application.Features.Authorization.Queries.GetPageGroups;
using MeUi.Application.Features.Authorization.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetPageGroupsEndpoint : BaseEndpointWithoutRequest<IEnumerable<PageGroupDto>>
{
    public override void ConfigureEndpoint()
    {
        Get("/page-groups");
        Description(x => x.WithTags("Authorization").WithSummary("Get all page groups"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<PageGroupDto> result = await Mediator.Send(new GetPageGroupsQuery(), ct);
        await SendSuccessAsync(result, "Page groups retrieved successfully", ct);
    }
}