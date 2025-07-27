using System.Net;

namespace MeUi.Shared.Endpoint.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string ErrorCode { get; set; }

    public ErrorResponse(HttpStatusCode statusCode, string message, string errorCode)
    {
        StatusCode = (int)statusCode;
        Message = message;
        ErrorCode = errorCode;
    }
}