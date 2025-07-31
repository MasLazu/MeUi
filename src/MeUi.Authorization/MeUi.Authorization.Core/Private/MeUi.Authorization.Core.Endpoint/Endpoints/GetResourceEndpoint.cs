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

public class GetResourcesEndpoint : EndpointWithAuthorizationWithoutRequest<GetResourcesEndpoint, SuccessResponse<IEnumerable<ResourceDto>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Get("/v1/resources");
        Description(x => x.WithTags("Resources"));
        Summary(s =>
        {
            s.Summary = "Get all available resources";
            s.Description = "Returns a list of defined resources in the authorization system. Each resource represents a protected entity that actions can be applied to in RBAC policies.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "RESOURCE";


    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<ResourceDto> resources = await new GetResourcesQuery().ExecuteAsync(ct);

        var response = new SuccessResponse<IEnumerable<ResourceDto>>(
            resources,
            "Resources retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}