using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authorization.Rbac.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Infrastructure.Data.Repositories;
using MeUi.Authorization.Rbac.Endpoint.Endpoints;
using MeUi.Authorization.Rbac.Application.QueryHandlers;

namespace MeUi.Authorization.Rbac.Extension;

public static class MainExtension
{
    public static IServiceCollection AddAuthorizationRbac(this IServiceCollection services, IConfiguration config)
    {
        _ = typeof(GetRoleByIdEndpoint).Assembly;
        _ = typeof(GetRoleByIdQueryHandler).Assembly;

        services.AddDbContext<AuthorizationRbacDbContext>((sp, opts) =>
        {
            IConfigurationSection postgresSection = config.GetSection("Postgresql");
            string connectionString = $"Host={postgresSection["Host"]};" +
                    $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
                    $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";
            opts.UseNpgsql(connectionString);
        });

        services.AddScoped(typeof(IAuthorizationRbacRepository<>), typeof(AuthorizationRbacRepository<>));

        return services;
    }
}