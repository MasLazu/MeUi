using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Commands.AssignUserRoles;

namespace MeUi.Api.Endpoints.Authorization;

public class AssignUserRolesRequest
{
    public Guid UserId { get; init; }
    public IEnumerable<Guid> RoleIds { get; init; } = [];
}

public class AssignUserRolesEndpoint : BaseEndpoint<AssignUserRolesRequest, IEnumerable<Guid>>
{
    protected override void ConfigureEndpoint()
    {
        Post("/users/{userId}/roles");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Assign roles to user")
            .WithDescription("Assigns one or more roles to a specific user"));
    }

    public override async Task HandleAsync(AssignUserRolesRequest req, CancellationToken ct)
    {
        var command = new AssignUserRolesCommand
        {
            UserId = req.UserId,
            RoleIds = req.RoleIds
        };

        var result = await Mediator.Send(command, ct);
        await SendSuccessAsync(result, ct);
    }
}