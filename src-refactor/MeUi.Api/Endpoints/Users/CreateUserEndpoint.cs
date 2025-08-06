using MeUi.Api.Common;
using MeUi.Application.Features.Users.Commands.CreateUser;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Api.Endpoints.Users;

public class CreateUserEndpoint : BaseEndpoint<CreateUserRequest, Guid>
{
    protected override void ConfigureEndpoint()
    {
        Post("/users");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Users")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user in the system"));
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var command = new CreateUserCommand
        {
            Username = req.Username,
            Email = req.Email,
            Name = req.Name
        };

        var userId = await Mediator.Send(command, ct);
        await SendSuccessAsync(userId, ct);
    }
}