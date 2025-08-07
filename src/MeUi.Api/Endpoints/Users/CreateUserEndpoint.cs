using MeUi.Api.Endpoints;
using MeUi.Application.Features.Users.Commands.CreateUser;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Api.Endpoints.Users;

public class CreateUserEndpoint : BaseEndpoint<CreateUserCommand, Guid>
{
    public override void ConfigureEndpoint()
    {
        Post("/users");
        Description(x => x.WithTags("Users").WithSummary("Create a new user"));
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        Guid userId = await Mediator.Send(req, ct);
        await SendSuccessAsync(userId, "User created successfully", ct);
    }
}