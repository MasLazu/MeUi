using MediatR;
using MeUi.Application.Common.Behaviors;

namespace MeUi.Application.Features.Users.Commands.DeleteUser;

[RequirePermission("delete:user")]
public record DeleteUserCommand : IRequest<Guid>
{
    public Guid Id { get; init; }
}