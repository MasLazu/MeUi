using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authentication.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Infrastructure.Services;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Shared.Infrastructure.Services;
using System.Reflection;
using MeUi.Authentication.Core.Infrastructure.Data.Repositories;

namespace MeUi.Authentication.Core.Extension;

public static class MainExtension
{
    public static IServiceCollection AddAuthenticationCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        IConfigurationSection postgresSection = config.GetSection("Postgresql");

        string connectionString = $"Host={postgresSection["Host"]};" +
            $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
            $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";

        services.AddDbContext<AuthenticationCoreDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IAuthenticationCoreRepository<>), typeof(AuthenticationCoreRepository<>));
        services.AddScoped<ILoginMethodSeeder, LoginMethodSeeder>();

        return services;
    }
}