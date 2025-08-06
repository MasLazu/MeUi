using MeUi.Api.Common;
using MeUi.Application.Features.Users.Commands.DeleteUser;

namespace MeUi.Api.Endpoints.Users;

public class DeleteUserRequest
{
    public Guid Id { get; set; }
}

public class DeleteUserEndpoint : BaseEndpoint<DeleteUserRequest, Guid>
{
    protected override void ConfigureEndpoint()
    {
        Delete("/users/{id}");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Users")
            .WithSummary("Delete a user")
            .WithDescription("Soft deletes a user from the system"));
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        var command = new DeleteUserCommand { Id = req.Id };
        var userId = await Mediator.Send(command, ct);
        await SendSuccessAsync(userId, ct);
    }
}