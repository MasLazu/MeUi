using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Authorization.Core.ApplicationContract.Queries;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authorization.Core.Endpoint.Endpoints;

public class GetActionsEndpoint : EndpointWithAuthorizationWithoutRequest<GetActionsEndpoint, SuccessResponse<IEnumerable<ActionDto>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Get("/v1/actions");
        Description(x => x.WithTags("Actions"));
        Summary(s =>
        {
            s.Summary = "Get all available actions";
            s.Description = "Returns a list of available actions that can be assigned to roles in the RBAC system. Each action represents an operation on a specific resource.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "ACTION";

    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<ActionDto> actions = await new GetActionsQuery().ExecuteAsync(ct);

        var response = new SuccessResponse<IEnumerable<ActionDto>>(
            actions,
            "Actions retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}