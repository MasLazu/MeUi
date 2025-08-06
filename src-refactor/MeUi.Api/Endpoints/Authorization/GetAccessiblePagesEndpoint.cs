using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Queries.GetAccessiblePages;
using MeUi.Application.Features.Authorization.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetAccessiblePagesRequest
{
    public Guid UserId { get; set; }
}

public class GetAccessiblePagesEndpoint : BaseEndpoint<GetAccessiblePagesRequest, IEnumerable<PageGroupDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/users/{userId}/accessible-pages");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Get accessible pages for user")
            .WithDescription("Retrieves pages that the user has access to based on their roles and permissions"));
    }

    public override async Task HandleAsync(GetAccessiblePagesRequest req, CancellationToken ct)
    {
        var query = new GetAccessiblePagesQuery
        {
            UserId = req.UserId
        };

        var result = await Mediator.Send(query, ct);
        await SendSuccessAsync(result, ct);
    }
}