using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authentication.Password.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authentication.Password.Endpoints.Endpoints;
using MeUi.Authentication.Password.Infrastructure.Data.seeders;
using MeUi.Authentication.Password.Application.CommandHandlers;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Infrastructure.Data.Repositories;
using System.Data.Common;

namespace MeUi.Authentication.Password.Extenstion;

public static class MainExtension
{
    public static IServiceCollection AddAuthenticationPassword(this IServiceCollection services, IConfiguration config)
    {
        _ = typeof(LoginEndpoint).Assembly;
        _ = typeof(LoginCommandHandler).Assembly;

        services.AddDbContext<AuthenticationPasswordDbContext>((sp, opts) =>
        {
            IConfigurationSection postgresSection = config.GetSection("Postgresql");
            string connectionString = $"Host={postgresSection["Host"]};" +
                    $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
                    $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";
            opts.UseNpgsql(connectionString);
        });

        services.AddScoped(typeof(IAuthenticationPasswordRepository<>), typeof(AuthenticationPasswordRepository<>));
        services.AddHostedService<PasswordLoginMethodSeeder>();

        return services;
    }
}