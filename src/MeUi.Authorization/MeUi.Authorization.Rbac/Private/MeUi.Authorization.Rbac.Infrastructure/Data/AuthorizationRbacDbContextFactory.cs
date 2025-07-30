using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MeUi.Authorization.Rbac.Infrastructure.Data;

public class AuthenticationRbacDbContextFactory : IDesignTimeDbContextFactory<AuthorizationRbacDbContext>
{
    public AuthorizationRbacDbContext CreateDbContext(string[] args)
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

        var optionsBuilder = new DbContextOptionsBuilder<AuthorizationRbacDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AuthorizationRbacDbContext(optionsBuilder.Options);
    }
}