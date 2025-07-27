using System.Net;

namespace MeUi.Shared.Application.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message = "Conflict")
        : base(message, (int)HttpStatusCode.Conflict) { }
}