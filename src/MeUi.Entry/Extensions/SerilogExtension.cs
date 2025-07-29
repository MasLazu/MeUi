using MeUi.Shared.Application.Exceptions;
using Serilog;
using Serilog.Events;
using System.Text.Json;

namespace MeUi.Entry.Extensions;

public static class SerilogExtension
{
    public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options => options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex is AppException || ex is FastEndpoints.ValidationFailureException || ex is JsonException || ex is FormatException)
                {
                    return LogEventLevel.Debug;
                }

                return ex != null ? LogEventLevel.Error : LogEventLevel.Information;
            });

        return app;
    }
}
