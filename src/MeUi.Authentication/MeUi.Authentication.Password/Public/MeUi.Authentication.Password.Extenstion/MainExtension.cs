using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authentication.Password.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authentication.Password.Endpoints.Endpoints;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Infrastructure.Data.Repositories;
using MeUi.Authentication.Password.Infrastructure.Data.seeders;
using MeUi.Authentication.Password.Application.CommandHandlers;

namespace MeUi.Authentication.Password.Extenstion;

public static class MainExtension
{
    public static IServiceCollection AddAuthenticationPassword(this IServiceCollection services, IConfiguration config)
    {
        _ = typeof(LoginEndpoint).Assembly;
        _ = typeof(LoginCommandHandler).Assembly;

        IConfigurationSection postgresSection = config.GetSection("Postgresql");

        string connectionString = $"Host={postgresSection["Host"]};" +
            $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
            $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";

        services.AddDbContext<AuthenticationPasswordDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddHostedService<PasswordLoginMethodSeeder>();

        return services;
    }
}