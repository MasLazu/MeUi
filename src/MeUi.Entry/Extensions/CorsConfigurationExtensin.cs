using System.Data.Common;
using Npgsql;

namespace MeUi.Entry.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var allowedOrigins = config
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy("Development", policy =>
            {
                policy.WithOrigins(allowedOrigins!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}