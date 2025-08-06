using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Queries.GetPermissions;
using MeUi.Application.Features.Authorization.Models;

namespace MeUi.Api.Endpoints.Authorization;

public class GetPermissionsEndpoint : BaseEndpointWithoutRequest<IEnumerable<PermissionDto>>
{
    protected override void ConfigureEndpoint()
    {
        Get("/permissions");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Get all permissions")
            .WithDescription("Retrieves all available permissions in the system"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetPermissionsQuery();
        var permissions = await Mediator.Send(query, ct);
        await SendSuccessAsync(permissions, ct);
    }
}