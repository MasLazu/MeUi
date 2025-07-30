using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Application.Spesifications;
using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeUi.Authorization.Core.Infrastructure.Data.Seeders;

public class ResourceActionSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public ResourceActionSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SeedAsync(CancellationToken ct)
    {
        IEnumerable<Type> providerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IResourceActionProvider).IsAssignableFrom(t) &&
                t.IsClass && !t.IsAbstract);

        using IServiceScope scope = _serviceProvider.CreateScope();

        IAuthorizationCoreRepository<ResourceAction> resourceActionRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationCoreRepository<ResourceAction>>();
        List<ResourceAction> resourceActions = await resourceActionRepository.ListAsync(new ResourceActionSpesification(), ct);

        var existingCodes = new HashSet<(string, string)>(resourceActions.Select(a => (a.ResourceCode, a.ActionCode)));

        foreach (Type providerType in providerTypes)
        {
            MethodInfo? resourceMethod = providerType.GetMethod("Resource", BindingFlags.Public | BindingFlags.Static);
            if (resourceMethod is null)
            {
                continue;
            }

            string? resource = resourceMethod.Invoke(null, null) as string;
            if (string.IsNullOrWhiteSpace(resource))
            {
                continue;
            }

            MethodInfo? actionMethod = providerType.GetMethod("Action", BindingFlags.Public | BindingFlags.Static);
            if (actionMethod is null)
            {
                continue;
            }

            string? action = actionMethod.Invoke(null, null) as string;
            if (string.IsNullOrWhiteSpace(action))
            {
                continue;
            }

            if (existingCodes.Add((resource, action)))
            {
                resourceActionRepository.Add(new ResourceAction
                {
                    ResourceCode = resource,
                    ActionCode = action
                }, ct);
            }
        }

        await resourceActionRepository.SaveChangesAsync(ct);
    }
}