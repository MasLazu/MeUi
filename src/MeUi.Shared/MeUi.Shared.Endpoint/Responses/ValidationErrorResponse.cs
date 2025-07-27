using System.Net;

namespace MeUi.Shared.Endpoint.Responses;

public class ValidationErrorResponse : ErrorResponse
{
    public IDictionary<string, string[]>? Errors { get; set; }

    public ValidationErrorResponse(IDictionary<string, string[]>? errors)
        : base(HttpStatusCode.BadRequest, "One or more validation errors occurred", "VALIDATION_ERROR")
    {
        Errors = errors;
    }
}
