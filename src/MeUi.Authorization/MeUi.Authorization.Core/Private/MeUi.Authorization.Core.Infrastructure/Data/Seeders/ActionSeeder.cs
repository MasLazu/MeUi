using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.Application.Spesifications;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeUi.Authorization.Core.Infrastructure.Data.Seeders;

public class ActionSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public ActionSeeder(IServiceProvider serviceProvider)
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

        IAuthorizationCoreRepository<Domain.Entities.Action> actionRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationCoreRepository<Domain.Entities.Action>>();
        List<Domain.Entities.Action> actions = await actionRepository.ListAsync(new ActionSpesification(), ct);

        var existingCodes = new HashSet<string>(actions.Select(a => a.Code));

        foreach (Type providerType in providerTypes)
        {
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

            if (existingCodes.Add(action))
            {
                actionRepository.Add(new Domain.Entities.Action
                {
                    Code = action,
                    Name = ToTitleCase(action)
                }, ct);
            }
        }

        await actionRepository.SaveChangesAsync(ct);
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