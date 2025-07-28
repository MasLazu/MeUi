using MeUi.Shared.Application.Exceptions;

namespace MeUi.Authentication.Core.Application.Exceptions;

public class InvalidRefreshTokenException : UnauthorizedException
{
    public InvalidRefreshTokenException() : base("The refresh token is invalid, expired, or has been revoked.") { }
}