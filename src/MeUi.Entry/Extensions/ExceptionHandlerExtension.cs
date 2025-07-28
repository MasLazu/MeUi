using MeUi.Entry.Middlewares;

namespace MeUi.Entry.Extensions;

public static class ExceptionHandlerExtension
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}