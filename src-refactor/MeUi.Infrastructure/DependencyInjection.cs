using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MeUi.Application.Common.Interfaces;
using MeUi.Infrastructure.Data;
using MeUi.Infrastructure.Data.Repositories;
using MeUi.Infrastructure.Services.Authentication;
using MeUi.Infrastructure.Services.Authorization;
using Npgsql;
using Microsoft.AspNetCore.Http;

namespace MeUi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        var connectionString = BuildConnectionString(configuration);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Repository Pattern
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Database Seeder
        services.AddScoped<Data.Seeders.DatabaseSeeder>();

        // Authentication Services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICurrentUser, CurrentUserService>();

        // Authorization Services
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        // HTTP Context Accessor for CurrentUser service
        services.AddHttpContextAccessor();

        return services;
    }

    private static string BuildConnectionString(IConfiguration configuration)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = configuration["Postgresql:Host"] ?? "localhost",
            Port = int.Parse(configuration["Postgresql:Port"] ?? "5432"),
            Username = configuration["Postgresql:Username"] ?? "postgres",
            Password = configuration["Postgresql:Password"] ?? "password",
            Database = configuration["Postgresql:Database"] ?? "meui_unified_db"
        };

        return builder.ConnectionString;
    }
}