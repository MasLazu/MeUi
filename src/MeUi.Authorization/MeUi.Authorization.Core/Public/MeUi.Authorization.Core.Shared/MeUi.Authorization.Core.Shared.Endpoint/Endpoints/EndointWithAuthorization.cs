using System.Net;
using FastEndpoints;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Shared.Endpoint.Endpoints;
using MeUi.Shared.Endpoint.Responses;

namespace MeUi.Authorization.Core.Shared.Endpoint.Endpoints;

public abstract class EndointWithAuthorization<TEnpoint, TRequest, TResponse> : BaseEndpoint<TRequest, TResponse>
    where TRequest : notnull where TEnpoint : IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
    }
}