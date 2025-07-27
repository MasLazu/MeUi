using System.Net;

namespace MeUi.Shared.Application.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized access")
        : base(message, (int)HttpStatusCode.Unauthorized) { }
}