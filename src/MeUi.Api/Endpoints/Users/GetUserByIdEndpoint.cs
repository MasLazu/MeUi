using MeUi.Api.Endpoints;
using MeUi.Application.Features.Users.Queries.GetUserById;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Api.Endpoints.Users;

public class GetUserByIdRequest
{
    public Guid Id { get; set; }
}

public class GetUserByIdEndpoint : BaseEndpoint<GetUserByIdRequest, UserDto>
{
    public override void ConfigureEndpoint()
    {
        Get("/users/{id}");
        Description(x => x.WithTags("Users").WithSummary("Get user by ID"));
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        var query = new GetUserByIdQuery { Id = req.Id };
        UserDto? user = await Mediator.Send(query, ct);

        if (user == null)
        {
            await SendErrorAsync("User not found", 404, ct);
            return;
        }

        await SendSuccessAsync(user, ct);
    }
}