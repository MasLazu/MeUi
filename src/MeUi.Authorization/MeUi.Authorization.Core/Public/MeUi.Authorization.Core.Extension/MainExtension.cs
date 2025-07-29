using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authorization.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Infrastructure.Data.Repositories;
using System.Data.Common;
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
        services.AddHostedService<ActionSeeder>();
        services.AddHostedService<ResourceSeeder>();
        services.AddHostedService<ResourceActionSeeder>();

        return services;
    }
}