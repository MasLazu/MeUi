using System.Net;

namespace MeUi.Shared.Endpoint.Responses;

public class SuccessResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public SuccessResponse(T data, string message = "Success", HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        StatusCode = (int)statusCode;
        Message = message;
        Data = data;
    }
}