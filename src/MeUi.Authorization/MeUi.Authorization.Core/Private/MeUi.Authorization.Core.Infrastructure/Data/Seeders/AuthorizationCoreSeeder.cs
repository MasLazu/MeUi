using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeUi.Authorization.Core.Infrastructure.Data.Seeders;

public class AuthorizationCoreSeeder : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationCoreSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        ResourceActionSeeder resourceActionSeeder = scope.ServiceProvider.GetRequiredService<ResourceActionSeeder>();
        ActionSeeder actionSeeder = scope.ServiceProvider.GetRequiredService<ActionSeeder>();
        ResourceSeeder resourceSeeder = scope.ServiceProvider.GetRequiredService<ResourceSeeder>();

        await resourceSeeder.SeedAsync(ct);
        await actionSeeder.SeedAsync(ct);
        await resourceActionSeeder.SeedAsync(ct);
    }
}