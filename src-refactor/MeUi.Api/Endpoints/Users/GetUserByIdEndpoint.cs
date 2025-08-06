using MeUi.Api.Common;
using MeUi.Application.Features.Users.Queries.GetUserById;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Api.Endpoints.Users;

public class GetUserByIdRequest
{
    public Guid Id { get; set; }
}

public class GetUserByIdEndpoint : BaseEndpoint<GetUserByIdRequest, UserDto>
{
    protected override void ConfigureEndpoint()
    {
        Get("/users/{id}");
        AllowAnonymous(); // TODO: Configure proper authorization
        Description(x => x
            .WithTags("Users")
            .WithSummary("Get user by ID")
            .WithDescription("Retrieves a user by their unique identifier"));
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        var query = new GetUserByIdQuery { Id = req.Id };
        var user = await Mediator.Send(query, ct);

        if (user == null)
        {
            await SendErrorAsync("User not found", 404, ct);
            return;
        }

        await SendSuccessAsync(user, ct);
    }
}