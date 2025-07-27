using MeUi.Shared.Application.Exceptions;

namespace MeUi.Authentication.Password.Application.Exceptions;

public class InvalidCredentialException : UnauthorizedException
{
    public InvalidCredentialException() : base("Invalid credential") { }
}