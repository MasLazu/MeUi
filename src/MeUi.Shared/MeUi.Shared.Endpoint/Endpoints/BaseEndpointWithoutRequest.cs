using System.Net;
using FastEndpoints;
using MeUi.Shared.Endpoint.Responses;

namespace MeUi.Shared.Endpoint.Endpoints;

public class BaseEndpointWithoutRequest<TResponse> : EndpointWithoutRequest<TResponse>
{
    public override void Configure()
    {
        DontCatchExceptions();
        Summary(s =>
        {
            s.ResponseExamples[400] = new ValidationErrorResponse(new Dictionary<string, string[]>());
            s.ResponseExamples[401] = new Responses.ErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized access", "UNAUTHORIZED");
            s.ResponseExamples[403] = new Responses.ErrorResponse(HttpStatusCode.Forbidden, "Forbidden access", "FORBIDDEN");
        });
    }
}