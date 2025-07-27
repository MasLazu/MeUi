using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    DateTime GetAccessTokenExpiration();
    RefreshToken GenerateRefreshToken(Guid userId);
}