using FastEndpoints;
using FastEndpoints.Swagger;
using MeUi.Api.Common;
using MeUi.Infrastructure.Data;
using MeUi.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MeUi.Api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures the HTTP request pipeline with proper middleware ordering
    /// </summary>
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure exception handling based on environment
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            Log.Information("Development environment detected - using developer exception page");
        }
        else
        {
            app.UseExceptionHandler();
            Log.Information("Production environment detected - using global exception handler");
        }

        // Request logging middleware (should be early in pipeline)
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Security headers and HTTPS redirection
        app.UseHttpsRedirection();

        // CORS middleware (must be before authentication/authorization)
        app.ConfigureCors();

        // Authentication & Authorization middleware (order matters)
        app.UseAuthentication();
        app.UseAuthorization();

        // Configure FastEndpoints and Swagger
        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
            c.Endpoints.ShortNames = true;
            c.Serializer.Options.PropertyNamingPolicy = null; // Keep original property names
            c.Endpoints.Configurator = ep =>
            {
                // Global endpoint configuration can be added here
                ep.Description(x => x.ClearDefaultProduces(200, 400));
            };
        })
        .UseSwaggerGen();

        Log.Information("FastEndpoints and Swagger documentation enabled");

        // Log successful pipeline configuration
        Log.Information("HTTP request pipeline configured successfully");

        return app;
    }

    /// <summary>
    /// Configures CORS based on environment
    /// </summary>
    private static WebApplication ConfigureCors(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("DevelopmentPolicy");
            Log.Information("Using development CORS policy (permissive)");
        }
        else
        {
            app.UseCors("DefaultPolicy");
            Log.Information("Using production CORS policy (restrictive)");
        }

        return app;
    }

    /// <summary>
    /// Initializes the database by applying migrations and seeding initial data
    /// </summary>
    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            Log.Information("Starting database initialization...");

            // Apply pending migrations
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            Log.Information("Database migrations applied successfully");

            // Seed initial data
            var seeder = services.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
            Log.Information("Database seeding completed successfully");

            Log.Information("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while initializing the database");
            throw;
        }

        return app;
    }
}