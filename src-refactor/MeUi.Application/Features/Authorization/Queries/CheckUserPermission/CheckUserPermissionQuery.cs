using MediatR;

namespace MeUi.Application.Features.Authorization.Queries.CheckUserPermission;

public record CheckUserPermissionQuery : IRequest<bool>
{
    public Guid UserId { get; init; }
    public string ResourceCode { get; init; } = string.Empty;
    public string ActionCode { get; init; } = string.Empty;
}