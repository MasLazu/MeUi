using FastEndpoints;
using MediatR;

namespace MeUi.Api.Common;

/// <summary>
/// Base endpoint class that provides common functionality for all endpoints
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
public abstract class BaseEndpoint<TRequest> : Endpoint<TRequest, ApiResponse>
    where TRequest : notnull, new()
{
    protected IMediator Mediator => Resolve<IMediator>();

    public override void Configure()
    {
        ConfigureEndpoint();
    }

    /// <summary>
    /// Override this method to configure the endpoint
    /// </summary>
    protected abstract void ConfigureEndpoint();

    /// <summary>
    /// Sends a successful response with data
    /// </summary>
    protected async Task SendSuccessAsync<T>(T data, CancellationToken ct = default)
    {
        await SendOkAsync(new ApiResponse<T>
        {
            Success = true,
            Data = data
        }, ct);
    }

    /// <summary>
    /// Sends a successful response without data
    /// </summary>
    protected async Task SendSuccessAsync(CancellationToken ct = default)
    {
        await SendOkAsync(new ApiResponse
        {
            Success = true,
            Message = "Operation completed successfully"
        }, ct);
    }

    /// <summary>
    /// Sends an error response
    /// </summary>
    protected async Task SendErrorAsync(string message, int statusCode = 400, CancellationToken ct = default)
    {
        await SendAsync(new ApiResponse
        {
            Success = false,
            Message = message
        }, statusCode, ct);
    }
}

/// <summary>
/// Base endpoint class with typed response
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response data type</typeparam>
public abstract class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, ApiResponse<TResponse>>
    where TRequest : notnull, new()
{
    protected IMediator Mediator => Resolve<IMediator>();

    public override void Configure()
    {
        ConfigureEndpoint();
    }

    /// <summary>
    /// Override this method to configure the endpoint
    /// </summary>
    protected abstract void ConfigureEndpoint();

    /// <summary>
    /// Sends a successful response with data
    /// </summary>
    protected async Task SendSuccessAsync(TResponse data, CancellationToken ct = default)
    {
        await SendOkAsync(new ApiResponse<TResponse>
        {
            Success = true,
            Data = data
        }, ct);
    }

    /// <summary>
    /// Sends an error response
    /// </summary>
    protected async Task SendErrorAsync(string message, int statusCode = 400, CancellationToken ct = default)
    {
        await SendAsync(new ApiResponse<TResponse>
        {
            Success = false,
            Message = message
        }, statusCode, ct);
    }
}

/// <summary>
/// Base endpoint class for endpoints without request body
/// </summary>
public abstract class BaseEndpointWithoutRequest : EndpointWithoutRequest<ApiResponse>
{
    protected IMediator Mediator => Resolve<IMediator>();

    public override void Configure()
    {
        ConfigureEndpoint();
    }

    /// <summary>
    /// Override this method to configure the endpoint
    /// </summary>
    protected abstract void ConfigureEndpoint();

    /// <summary>
    /// Sends a successful response with data
    /// </summary>
    protected async Task SendSuccessAsync<T>(T data, CancellationToken ct = default)
    {
        await SendOkAsync(new ApiResponse<T>
        {
            Success = true,
            Data = data
        }, ct);
    }

    /// <summary>
    /// Sends a successful response without data
    /// </summary>
    protected async Task SendSuccessAsync(CancellationToken ct = default)
    {
        await SendOkAsync(new ApiResponse
        {
            Success = true,
            Message = "Operation completed successfully"
        }, ct);
    }

    /// <summary>
    /// Sends an error response
    /// </summary>
    protected async Task SendErrorAsync(string message, int statusCode = 400, CancellationToken ct = default)
    {
        await SendAsync(new ApiResponse
        {
            Success = false,
            Message = message
        }, statusCode, ct);
    }
}

/// <summary>
/// Base endpoint class for endpoints without request body but with typed response
/// </summary>
/// <typeparam name="TResponse">The response data type</typeparam>
public abstract class BaseEndpointWithoutRequest<TResponse> : EndpointWithoutRequest<ApiResponse<TResponse>>
{
    protected IMediator Mediator => Resolve<IMediator>();

    public override void Configure()
    {
        ConfigureEndpoint();
    }

    /// <summary>
    /// Override this method to configure the endpoint
    /// </summary>
    protected abstract void ConfigureEndpoint();

    /// <summary>
    /// Sends a successful response with data
    /// </summary>
    protected async Task SendSuccessAsync(TResponse data, CancellationToken ct = default)
    {
        await SendOkAsync(new ApiResponse<TResponse>
        {
            Success = true,
            Data = data
        }, ct);
    }

    /// <summary>
    /// Sends an error response
    /// </summary>
    protected async Task SendErrorAsync(string message, int statusCode = 400, CancellationToken ct = default)
    {
        await SendAsync(new ApiResponse<TResponse>
        {
            Success = false,
            Message = message
        }, statusCode, ct);
    }
}