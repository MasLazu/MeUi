using System.Diagnostics;
using System.Text;
using Serilog;

namespace MeUi.Api.Common;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<RequestLoggingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];

        // Log request
        await LogRequestAsync(context, requestId);

        // Capture response
        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            // Log response
            await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds);

            // Copy response back to original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
    }

    private async Task LogRequestAsync(HttpContext context, string requestId)
    {
        var request = context.Request;
        var requestBody = string.Empty;

        // Read request body for POST/PUT requests
        if (request.Method == "POST" || request.Method == "PUT" || request.Method == "PATCH")
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength ?? 0)];
            await request.Body.ReadExactlyAsync(buffer, 0, buffer.Length);
            requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;
        }

        _logger.Information(
            "HTTP {Method} {Path} started. RequestId: {RequestId}, User: {User}, ContentType: {ContentType}, Body: {RequestBody}",
            request.Method,
            request.Path,
            requestId,
            context.User?.Identity?.Name ?? "Anonymous",
            request.ContentType,
            SanitizeRequestBody(requestBody));
    }

    private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMs)
    {
        var response = context.Response;
        var responseBody = string.Empty;

        // Read response body
        if (response.Body.CanSeek)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
        }

        var logLevel = response.StatusCode >= 400 ? Serilog.Events.LogEventLevel.Warning : Serilog.Events.LogEventLevel.Information;

        _logger.Write(logLevel,
            "HTTP {Method} {Path} completed. RequestId: {RequestId}, StatusCode: {StatusCode}, Duration: {Duration}ms, ResponseBody: {ResponseBody}",
            context.Request.Method,
            context.Request.Path,
            requestId,
            response.StatusCode,
            elapsedMs,
            SanitizeResponseBody(responseBody));
    }

    private static string SanitizeRequestBody(string body)
    {
        if (string.IsNullOrEmpty(body))
            return string.Empty;

        // Remove sensitive information like passwords
        if (body.Contains("password", StringComparison.OrdinalIgnoreCase))
        {
            return "[SENSITIVE DATA REMOVED]";
        }

        // Limit body size for logging
        return body.Length > 1000 ? body[..1000] + "..." : body;
    }

    private static string SanitizeResponseBody(string body)
    {
        if (string.IsNullOrEmpty(body))
            return string.Empty;

        // Limit response body size for logging
        return body.Length > 1000 ? body[..1000] + "..." : body;
    }
}