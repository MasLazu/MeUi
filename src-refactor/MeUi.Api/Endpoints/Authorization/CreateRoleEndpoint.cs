using MeUi.Api.Common;
using MeUi.Application.Features.Authorization.Commands.CreateRole;

namespace MeUi.Api.Endpoints.Authorization;

public class CreateRoleRequest
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public class CreateRoleEndpoint : BaseEndpoint<CreateRoleRequest, Guid>
{
    protected override void ConfigureEndpoint()
    {
        Post("/roles");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Authorization")
            .WithSummary("Create a new role")
            .WithDescription("Creates a new role in the system"));
    }

    public override async Task HandleAsync(CreateRoleRequest req, CancellationToken ct)
    {
        var command = new CreateRoleCommand
        {
            Code = req.Code,
            Name = req.Name,
            Description = req.Description
        };

        var roleId = await Mediator.Send(command, ct);
        await SendSuccessAsync(roleId, ct);
    }
}