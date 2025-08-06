using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using MeUi.Api.Common;

namespace MeUi.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds API-specific services including FastEndpoints, Swagger, CORS, and Authentication
    /// </summary>
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Authentication & Authorization
        services.AddAuthenticationJwtBearer(s => s.SigningKey = configuration["Jwt:Key"]!)
                .AddAuthorization();

        // Add FastEndpoints
        services.AddFastEndpoints();

        // Add Swagger documentation
        services.AddSwaggerDocumentation();

        // Add CORS policies
        services.AddCorsConfiguration(configuration);

        return services;
    }

    /// <summary>
    /// Adds global exception handling services
    /// </summary>
    public static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    /// <summary>
    /// Configures Swagger documentation
    /// </summary>
    private static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddFastEndpoints().SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.DocumentName = "v1";
                s.Title = "MeUi API";
                s.Version = "v1";
                s.Description = "MeUi Application API - Unified Monolith with MediatR CQRS";
            };
        });

        return services;
    }

    /// <summary>
    /// Configures CORS policies using configuration values
    /// </summary>
    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            // Production CORS policy - uses configuration
            options.AddPolicy("DefaultPolicy", policy =>
            {
                var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                    ?? new[] { "http://localhost:3000", "https://localhost:3001" };

                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });

            // Development CORS policy (more permissive)
            options.AddPolicy("DevelopmentPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}