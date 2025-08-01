using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authentication.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Infrastructure.Services;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Infrastructure.Data.Repositories;
using System.Data.Common;
using MeUi.Authentication.Core.Endpoint.Endpoints;
using MeUi.Authentication.Core.Application.QueryHandlers;
using MeUi.Authentication.Core.Infrastructure.Data.Seeders;

namespace MeUi.Authentication.Core.Extension;

public static class MainExtension
{
    public static IServiceCollection AddAuthenticationCore(this IServiceCollection services, IConfiguration config)
    {
        _ = typeof(GetActiveLoginMethodsEndpoint).Assembly;
        _ = typeof(GetActiveLoginMethodsQueryHandler).Assembly;

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddDbContext<AuthenticationCoreDbContext>((sp, opts) =>
        {
            IConfigurationSection postgresSection = config.GetSection("Postgresql");
            string connectionString = $"Host={postgresSection["Host"]};" +
                    $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
                    $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";
            opts.UseNpgsql(connectionString);
        });

        services.AddScoped(typeof(IAuthenticationCoreRepository<>), typeof(AuthenticationCoreRepository<>));
        services.AddScoped<ILoginMethodSeeder, LoginMethodSeeder>();
        services.AddScoped<IUserSeeder, UserSeeder>();
        services.AddScoped<IUserLoginMethodSeeder, UserLoginMethodSeeder>();

        return services;
    }
}