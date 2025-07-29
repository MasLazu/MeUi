using System.Data.Common;
using Npgsql;

namespace MeUi.Entry.Extensions;

public static class DbConnectionExtension
{
    public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration config)
    {
        // services.AddScoped<DbConnection>(sp =>
        // {
        //     IConfiguration cfg = sp.GetRequiredService<IConfiguration>();
        //     IConfigurationSection postgresSection = cfg.GetSection("Postgresql");
        //     string connectionString = $"Host={postgresSection["Host"]};" +
        //             $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
        //             $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";
        //     return new NpgsqlConnection(connectionString);
        // });

        // IConfigurationSection postgresSection = config.GetSection("Postgresql");
        // string connectionString = $"Host={postgresSection["Host"]};" +
        //         $"Port={postgresSection["Port"]};" + $"Username={postgresSection["Username"]};" +
        //         $"Password={postgresSection["Password"]};" + $"Database={postgresSection["Database"]};";
        return services;
    }
}