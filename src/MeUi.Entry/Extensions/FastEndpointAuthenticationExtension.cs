using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MeUi.Entry.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddFastEndpointAuthentication(this IServiceCollection services, IConfiguration config)
    {
        string jwtSecret = config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
        string jwtIssuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        string jwtAudience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret))
            });

        services.AddAuthorization();
        return services;
    }
}