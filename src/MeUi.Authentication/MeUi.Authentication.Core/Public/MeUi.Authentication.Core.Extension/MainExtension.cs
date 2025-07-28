using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MeUi.Authentication.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Infrastructure.Services;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Shared.Infrastructure.Services;
using MeUi.Authentication.Core.Infrastructure.Data.Repositories;
using System.Data.Common;

namespace MeUi.Authentication.Core.Extension;

public static class MainExtension
{
    public static IServiceCollection AddAuthenticationCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddDbContext<AuthenticationCoreDbContext>((sp, opts) =>
        {
            var conn = sp.GetRequiredService<DbConnection>();
            opts.UseNpgsql(conn);
        });

        services.AddScoped(typeof(IAuthenticationCoreRepository<>), typeof(AuthenticationCoreRepository<>));
        services.AddScoped<ILoginMethodSeeder, LoginMethodSeeder>();

        return services;
    }
}