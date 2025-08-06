using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Queries.GetUserPermissions;
using MeUi.Application.Features.Authorization.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetUserPermissionsRequest
{
    public Guid UserId { get; set; }
}

public class GetUserPermissionsEndpoint : BaseEndpoint<GetUserPermissionsRequest, IEnumerable<PermissionDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/users/{userId}/permissions");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Get user permissions")
            .WithDescription("Retrieves all permissions for a specific user"));
    }

    public override async Task HandleAsync(GetUserPermissionsRequest req, CancellationToken ct)
    {
        var query = new GetUserPermissionsQuery(req.UserId);
        var permissions = await Mediator.Send(query, ct);
        await SendSuccessAsync(permissions, ct);
    }
}