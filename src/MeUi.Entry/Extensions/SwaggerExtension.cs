using FastEndpoints;
using FastEndpoints.Swagger;

namespace MeUi.Entry.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddFastEndpointsSwagger(this IServiceCollection services, IConfiguration config)
    {
        services.AddFastEndpoints().SwaggerDocument(o => o.DocumentSettings = s =>
            {
                s.DocumentName = "v1";
                s.Title = "MataElang Defense Center Ui API";
                s.Version = "v1";
            });
        return services;
    }
}