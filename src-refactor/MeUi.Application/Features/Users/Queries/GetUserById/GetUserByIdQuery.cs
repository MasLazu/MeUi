using MediatR;
using MeUi.Application.Common.Behaviors;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Application.Features.Users.Queries.GetUserById;

[RequirePermission("read:user")]
public record GetUserByIdQuery : IRequest<UserDto?>
{
    public Guid Id { get; init; }
}