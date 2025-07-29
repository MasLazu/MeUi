using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MeUi.Authorization.Core.Infrastructure.Data;

public class AuthenticationCoreDbContextFactory : IDesignTimeDbContextFactory<AuthorizationCoreDbContext>
{
    public AuthorizationCoreDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string? host = config["Postgresql:Host"];
        string? port = config["Postgresql:Port"];
        string? user = config["Postgresql:Username"];
        string? pass = config["Postgresql:Password"];
        string? db = config["Postgresql:Database"];

        string connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={db}";

        var optionsBuilder = new DbContextOptionsBuilder<AuthorizationCoreDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AuthorizationCoreDbContext(optionsBuilder.Options);
    }
}