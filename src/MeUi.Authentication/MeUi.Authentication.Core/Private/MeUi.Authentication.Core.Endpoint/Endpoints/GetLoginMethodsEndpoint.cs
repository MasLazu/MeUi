using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class GetLoginMethodsEndpoint : BaseEndpointWithoutRequest<SuccessResponse<IEnumerable<LoginMethodDto>>>
{
    public override void Configure()
    {
        base.Configure();
        Get("/v1/auth/login-methods");
        AllowAnonymous();
        Description(x => x.WithTags("Auth"));
        Summary(s =>
        {
            s.Summary = "Get login methods";
            s.Description = "Returns a list of currently active and supported login methods such as password-based login, OAuth, or other authentication mechanisms configured in the system.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<LoginMethodDto> loginMethods = await new GetLoginMethodsQuery().ExecuteAsync(ct);

        var response = new SuccessResponse<IEnumerable<LoginMethodDto>>(
            loginMethods,
            "Login methods retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}