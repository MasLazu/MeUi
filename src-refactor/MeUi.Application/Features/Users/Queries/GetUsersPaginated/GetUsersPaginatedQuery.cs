using MediatR;
using MeUi.Application.Common.Behaviors;
using MeUi.Application.Common.Models;
using MeUi.Application.Features.Users.Models;

namespace MeUi.Application.Features.Users.Queries.GetUsersPaginated;

[RequirePermission("read:user")]
public record GetUsersPaginatedQuery : IRequest<PaginatedResult<UserDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
    public bool? IsSuspended { get; init; }
}