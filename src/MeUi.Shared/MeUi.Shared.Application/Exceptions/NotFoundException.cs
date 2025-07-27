using System.Net;

namespace MeUi.Shared.Application.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string entityName)
        : base($"{entityName} not found", (int)HttpStatusCode.NotFound) { }
}