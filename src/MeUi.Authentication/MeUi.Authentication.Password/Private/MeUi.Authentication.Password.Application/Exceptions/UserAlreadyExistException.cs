using MeUi.Shared.Application.Exceptions;

namespace MeUi.Authentication.Password.Application.Exceptions;

public class UserAlreadyExistException : ConflictException
{
    public UserAlreadyExistException(string propertyName) : base($"{propertyName} already used") { }
}