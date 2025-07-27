using System.Security.Claims;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Domain.Entities;
using Microsoft.Extensions.Configuration;
using FastEndpoints.Security;
using System.Security.Cryptography;

namespace MeUi.Authentication.Core.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new ("sub", user.Id.ToString()),
            new("name", user.Name),
        };

        if (user.Email != null)
        {
            claims.Add(new("email", user.Email));
        }

        if (user.Username != null)
        {
            claims.Add(new("username", user.Username));
        }

        return JwtBearer.CreateToken(o =>
        {
            o.SigningKey = GetJwtSecret();
            o.Issuer = GetJwtIssuer();
            o.Audience = GetJwtAudience();
            o.User.Claims.AddRange(claims);
        });
    }

    public DateTime GetAccessTokenExpiration()
    {
        string? expirationMinutesStr = _configuration["Jwt:AccessTokenExpirationMinutes"];
        int expirationMinutes = string.IsNullOrEmpty(expirationMinutesStr) ? 15 : int.Parse(expirationMinutesStr);
        return DateTime.UtcNow.AddMinutes(expirationMinutes);
    }

    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        string? expirationDaysStr = _configuration["Jwt:RefreshTokenExpirationDays"];
        int expirationDays = string.IsNullOrEmpty(expirationDaysStr) ? 7 : int.Parse(expirationDaysStr);

        byte[] randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        string tokenValue = Convert.ToBase64String(randomNumber);

        return new RefreshToken()
        {
            Id = Guid.CreateVersion7(),
            Token = tokenValue,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
            CreatedAt = DateTime.UtcNow
        };
    }

    private string GetJwtSecret()
    {
        string? secret = _configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JWT secret is not configured");
        }
        return secret;
    }

    private string GetJwtIssuer()
    {
        return _configuration["Jwt:Issuer"] ?? "MataElang.DefenseCenter";
    }

    private string GetJwtAudience()
    {
        return _configuration["Jwt:Audience"] ?? "MataElang.DefenseCenter.Users";
    }
}