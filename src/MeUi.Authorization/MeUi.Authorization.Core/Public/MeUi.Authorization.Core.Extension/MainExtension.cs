using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authorization.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Infrastructure.Data.Repositories;
using MeUi.Authorization.Core.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.QueryHandlers;
using MeUi.Authorization.Core.Infrastructure.Data.Seeders;

namespace MeUi.Authorization.Core.Extension;

public static class MainExtension
{
    public static IServiceCollection AddAuthorizationCore(this IServiceCollection services, IConfiguration config)
    {
        _ = typeof(GetResourceActionEndpoint).Assembly;
        _ = typeof(GetResourceActionsQueryHandler).Assembly;

        services.AddDbContext<AuthorizationCoreDbContext>((sp, opts) =>
        {
            IConfigurationSection postgresSection = config.GetSection("Postgresql");
            string connectionString = $"Host={postgresSection["Host"]};" +
                    $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
                    $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";
            opts.UseNpgsql(connectionString);
        });

        services.AddScoped(typeof(IAuthorizationCoreRepository<>), typeof(AuthorizationCoreRepository<>));
        services.AddScoped<ActionSeeder>();
        services.AddScoped<ResourceSeeder>();
        services.AddScoped<ResourceActionSeeder>();
        services.AddHostedService<AuthorizationCoreSeeder>();

        return services;
    }
}