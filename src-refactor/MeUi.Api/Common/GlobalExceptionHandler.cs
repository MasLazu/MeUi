using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using MeUi.Application.Common.Exceptions;

namespace MeUi.Api.Common;

/// <summary>
/// Global exception handler for consistent error responses
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        httpContext.Response.ContentType = "application/json";

        var (statusCode, message, errorCode, errors) = MapException(exception);

        var response = CreateErrorResponse(statusCode, message, errorCode, errors);

        httpContext.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(response, jsonOptions),
            cancellationToken);

        return true;
    }

    private (HttpStatusCode statusCode, string message, string errorCode, IEnumerable<string>? errors) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                notFoundEx.Message,
                notFoundEx.ErrorCode,
                notFoundEx.Errors
            ),
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                validationEx.Message,
                validationEx.ErrorCode,
                validationEx.Errors
            ),
            ConflictException conflictEx => (
                HttpStatusCode.Conflict,
                conflictEx.Message,
                conflictEx.ErrorCode,
                conflictEx.Errors
            ),
            UnauthorizedException unauthorizedEx => (
                HttpStatusCode.Unauthorized,
                unauthorizedEx.Message,
                unauthorizedEx.ErrorCode,
                unauthorizedEx.Errors
            ),
            ForbiddenException forbiddenEx => (
                HttpStatusCode.Forbidden,
                forbiddenEx.Message,
                forbiddenEx.ErrorCode,
                forbiddenEx.Errors
            ),
            FastEndpoints.ValidationFailureException fastEndpointValidationEx => (
                HttpStatusCode.BadRequest,
                "One or more validation errors occurred",
                "VALIDATION_ERROR",
                fastEndpointValidationEx.Failures?.Select(f => f.ErrorMessage)
            ),
            JsonException jsonEx => (
                HttpStatusCode.BadRequest,
                "Invalid JSON format",
                "JSON_ERROR",
                new[] { jsonEx.Message }
            ),
            FormatException formatEx => (
                HttpStatusCode.BadRequest,
                "Invalid format",
                "FORMAT_ERROR",
                new[] { formatEx.Message }
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred",
                "INTERNAL_SERVER_ERROR",
                null
            )
        };
    }

    private ApiResponse CreateErrorResponse(HttpStatusCode statusCode, string message, string errorCode, IEnumerable<string>? errors)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors != null ? new
            {
                General = errors.ToArray()
            } : new
            {
                ErrorCode = errorCode,
                StatusCode = (int)statusCode
            }
        };
    }
}