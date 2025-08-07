using FastEndpoints.Security;
using MeUi.Api.Endpoints;
using MeUi.Application.Features.Authorization.Commands.AssignUserRoles;

namespace MeUi.Api.Endpoints.Authorization;

public class AssignUserRolesEndpoint : BaseEndpoint<AssignUserRolesCommand, IEnumerable<Guid>>
{
    public override void ConfigureEndpoint()
    {
        Post("/users/{userId}/roles");
        Description(x => x.WithTags("Authorization").WithSummary("Assign roles to user"));
    }

    public override async Task HandleAsync(AssignUserRolesCommand req, CancellationToken ct)
    {
        IEnumerable<Guid> result = await Mediator.Send(req, ct);
        await SendSuccessAsync(result, "Roles assigned successfully", ct);
    }
}