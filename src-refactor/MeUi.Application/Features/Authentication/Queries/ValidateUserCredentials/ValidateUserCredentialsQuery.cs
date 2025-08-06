using MediatR;

namespace MeUi.Application.Features.Authentication.Queries.ValidateUserCredentials;

public record ValidateUserCredentialsQuery(
    string Email,
    string Password,
    string LoginMethodCode
) : IRequest<bool>;