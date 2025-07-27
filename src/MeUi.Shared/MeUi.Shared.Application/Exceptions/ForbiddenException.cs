using System.Net;

namespace MeUi.Shared.Application.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "Forbidden access")
        : base(message, (int)HttpStatusCode.Forbidden) { }
}