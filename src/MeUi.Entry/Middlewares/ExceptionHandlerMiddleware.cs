using System.Net;
using System.Text.Json;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Endpoint.Responses;

namespace MeUi.Entry.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";
            IDictionary<string, string[]>? validationErrors = null;

            switch (ex)
            {
                case FastEndpoints.ValidationFailureException validationException:
                    _logger.LogInformation("Validation error occurred: {Message}", validationException.Message);
                    context.Response.ContentType = "application/problem+json";
                    statusCode = HttpStatusCode.BadRequest;
                    message = "One or more validation errors occurred.";
                    validationErrors = validationException.Failures?
                                .GroupBy(e => e.PropertyName)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(e => e.ErrorMessage).ToArray()
                                );
                    break;
                case AppException appException:
                    statusCode = (HttpStatusCode)appException.StatusCode;
                    message = appException.Message;
                    validationErrors = appException.Errors?.ToDictionary(e => "General", e => new string[] { e });
                    _logger.LogInformation("Application error (client-side): {Message}", appException.Message);
                    break;
                case JsonException jsonException:
                    _logger.LogInformation("JSON deserialization error: {Message}", jsonException.Message);
                    statusCode = HttpStatusCode.BadRequest;
                    message = jsonException.Message;
                    break;
                case FormatException formatException:
                    _logger.LogInformation("Format error: {Message}", formatException.Message);
                    statusCode = HttpStatusCode.BadRequest;
                    message = formatException.Message;
                    break;
                default:
                    _logger.LogWarning(ex, "An unhandled exception occurred: {Message}", ex.Message);
                    break;
            }

            object errorResponse;
            string errorCode = statusCode != HttpStatusCode.InternalServerError ?
                ToScreamingSnakeCase(ex.GetType().Name.Replace("Exception", string.Empty)) :
                "INTERNAL_SERVER_ERROR";

            errorResponse = validationErrors is not null
                ? new ValidationErrorResponse(validationErrors)
                : (object)new ErrorResponse(statusCode, message, errorCode);

            string result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(result);
        }
    }

    private static string ToScreamingSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var sb = new System.Text.StringBuilder();
        bool prevUpper = true;

        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                if (!prevUpper)
                {
                    sb.Append('_');
                }
                sb.Append(char.ToUpper(c));
                prevUpper = true;
            }
            else
            {
                sb.Append(char.ToUpper(c));
                prevUpper = false;
            }
        }

        return sb.ToString();
    }
}