using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Behaviors;
using MeUi.Application.Common.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeUi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR with all assemblies
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            // Register from all application assemblies to ensure all handlers are found
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName?.StartsWith("MeUi.Application") == true)
                .ToArray());
        });

        // Register pipeline behaviors in order
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        // Add FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Add Mapster
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        MappingConfig.Configure();
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}