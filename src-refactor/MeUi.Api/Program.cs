using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using MeUi.Api.Common;
using MeUi.Api.Extensions;
using MeUi.Application;
using MeUi.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Configure Serilog early for startup logging
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Starting MeUi API application");

    // Add services to the container using extension methods
    builder.Services
        .AddApplication()                                    // MediatR, behaviors, validation, mapping
        .AddInfrastructure(builder.Configuration)           // Database, repositories, external services
        .AddApiServices(builder.Configuration)              // FastEndpoints, Swagger, CORS, Auth
        .AddGlobalExceptionHandling();                      // Global exception handling

    var app = builder.Build();

    // Configure the HTTP request pipeline
    app.ConfigurePipeline();

    // Initialize database (apply migrations and seed data)
    await app.InitializeDatabaseAsync();

    Log.Information("MeUi API application configured successfully");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}