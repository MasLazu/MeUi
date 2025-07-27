using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class RefreshTokenByIdSpesification : Specification<RefreshToken>
{
    public RefreshTokenByIdSpesification(Guid id)
    {
        Query.Where(rt => rt.Id == id && rt.DeletedAt == null);
    }
}
