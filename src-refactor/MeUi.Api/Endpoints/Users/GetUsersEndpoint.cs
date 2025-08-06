using MeUi.Api.Common;
using MeUi.Application.Features.Users.Queries.GetUsersPaginated;
using MeUi.Application.Common.Models;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Api.Endpoints.Users;

public class GetUsersRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public bool? IsSuspended { get; set; }
}

public class GetUsersEndpoint : BaseEndpoint<GetUsersRequest, PaginatedResult<UserDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/users");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Users")
            .WithSummary("Get paginated list of users")
            .WithDescription("Retrieves a paginated list of users with optional filtering"));
    }

    public override async Task HandleAsync(GetUsersRequest req, CancellationToken ct)
    {
        var query = new GetUsersPaginatedQuery
        {
            PageNumber = req.PageNumber,
            PageSize = req.PageSize,
            SearchTerm = req.SearchTerm,
            IsSuspended = req.IsSuspended
        };

        var result = await Mediator.Send(query, ct);
        await SendSuccessAsync(result, ct);
    }
}