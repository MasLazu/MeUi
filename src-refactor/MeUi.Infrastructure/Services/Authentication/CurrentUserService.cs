using System.Security.Claims;
using MeUi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MeUi.Infrastructure.Services.Authentication;

public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirst("username")?.Value;

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
                .Select(c => c.Value) ?? Enumerable.Empty<string>();
        }
    }

    public IEnumerable<string> Permissions
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindAll("permission")
                .Select(c => c.Value) ?? Enumerable.Empty<string>();
        }
    }
}