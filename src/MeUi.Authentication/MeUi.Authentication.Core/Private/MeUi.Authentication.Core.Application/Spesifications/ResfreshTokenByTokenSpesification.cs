using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class RefreshTokenByTokenSpesification : Specification<RefreshToken>
{
    public RefreshTokenByTokenSpesification(string token)
    {
        Query.Where(rt => rt.Token == token && rt.DeletedAt == null);
    }
}
