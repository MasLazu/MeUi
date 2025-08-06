using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Queries.GetPages;
using MeUi.Application.Features.Authorization.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetPagesEndpoint : BaseEndpointWithoutRequest<IEnumerable<PageDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/pages");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Get all pages")
            .WithDescription("Retrieves all pages in the system"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetPagesQuery();
        var result = await Mediator.Send(query, ct);
        await SendSuccessAsync(result, ct);
    }
}