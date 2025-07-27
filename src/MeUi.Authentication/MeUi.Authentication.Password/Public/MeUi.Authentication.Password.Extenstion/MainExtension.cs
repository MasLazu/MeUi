using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authentication.Password.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MeUi.Authentication.Password.Endpoints.Endpoints;

namespace MeUi.Authentication.Password.Extenstion;

public static class MainExtension
{
    public static IServiceCollection AddAuthenticationPassword(this IServiceCollection services, IConfiguration config)
    {
        _ = typeof(LoginEndpoint).Assembly;

        IConfigurationSection postgresSection = config.GetSection("Postgresql");

        string connectionString = $"Host={postgresSection["Host"]};" +
            $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
            $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";

        services.AddDbContext<AuthenticationPasswordDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}