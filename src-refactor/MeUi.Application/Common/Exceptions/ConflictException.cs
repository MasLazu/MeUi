using System.Net;

namespace MeUi.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a conflict occurs (e.g., duplicate resource)
/// </summary>
public class ConflictException : ApplicationException
{
    public ConflictException(string message = "Conflict")
        : base(message, (int)HttpStatusCode.Conflict) { }
}