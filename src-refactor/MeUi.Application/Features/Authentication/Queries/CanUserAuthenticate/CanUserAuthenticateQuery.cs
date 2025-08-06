using MediatR;

namespace MeUi.Application.Features.Authentication.Queries.CanUserAuthenticate;

public record CanUserAuthenticateQuery(
    Guid UserId,
    string LoginMethodCode
) : IRequest<bool>;