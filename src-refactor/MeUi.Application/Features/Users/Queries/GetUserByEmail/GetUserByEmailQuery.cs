using MediatR;
using MeUi.Application.Common.Behaviors;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Application.Features.Users.Queries.GetUserByEmail;

[RequirePermission("read:user")]
public record GetUserByEmailQuery : IRequest<UserDto?>
{
    public string Email { get; init; } = string.Empty;
}