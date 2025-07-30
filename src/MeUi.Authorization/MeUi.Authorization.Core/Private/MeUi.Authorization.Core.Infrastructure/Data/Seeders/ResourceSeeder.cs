using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Application.Spesifications;
using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace MeUi.Authorization.Core.Infrastructure.Data.Seeders;

public class ResourceSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public ResourceSeeder(IServiceProvider serviceProvider)
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

        IAuthorizationCoreRepository<Resource> resourceRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationCoreRepository<Resource>>();
        List<Resource> resources = await resourceRepository.ListAsync(new ResourceSpesification(), ct);

        var existingCodes = new HashSet<string>(resources.Select(a => a.Code));

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

            if (existingCodes.Add(resource))
            {
                resourceRepository.Add(new Resource
                {
                    Code = resource,
                    Name = ToTitleCase(resource)
                }, ct);
            }
        }

        await resourceRepository.SaveChangesAsync(ct);
    }

    private static string ToTitleCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return string.Join(" ",
        input
            .Split('_', StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant()));
    }
}