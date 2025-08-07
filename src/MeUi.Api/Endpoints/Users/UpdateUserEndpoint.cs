using MeUi.Api.Endpoints;
using MeUi.Application.Features.Users.Commands.UpdateUser;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Api.Endpoints.Users;

public class UpdateUserEndpoint : BaseEndpoint<UpdateUserRequest, Guid>
{
    protected override void ConfigureEndpoint()
    {
        Put("/users/{id}");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Users")
            .WithSummary("Update an existing user")
            .WithDescription("Updates an existing user in the system"));
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var command = new UpdateUserCommand
        {
            Id = req.Id,
            Username = req.Username,
            Email = req.Email,
            Name = req.Name,
            IsSuspended = req.IsSuspended
        };

        Guid userId = await Mediator.Send(command, ct);
        await SendSuccessAsync(userId, ct);
    }
}