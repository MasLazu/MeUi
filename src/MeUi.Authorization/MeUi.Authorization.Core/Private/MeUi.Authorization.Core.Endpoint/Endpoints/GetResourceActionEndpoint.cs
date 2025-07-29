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

public class GetResourceActionEndpoint : EndpointWithAuthorizationWithoutRequest<GetResourceActionEndpoint, SuccessResponse<IEnumerable<ResourceActionDto>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Get("/v1/resource-actions");
        Description(x => x.WithTags("ResourceActions"));
        Summary(s =>
        {
            s.Summary = "Get all resource-action mappings";
            s.Description = "Returns a list of resource-action pairs that define which actions are applicable to which resources. This is used to build or evaluate RBAC policies.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "RESOURCE_ACTION";

    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<ResourceActionDto> resourceActions = await new GetResourceActionsQuery().ExecuteAsync(ct);

        var response = new SuccessResponse<IEnumerable<ResourceActionDto>>(
            resourceActions,
            "Resource actions retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}