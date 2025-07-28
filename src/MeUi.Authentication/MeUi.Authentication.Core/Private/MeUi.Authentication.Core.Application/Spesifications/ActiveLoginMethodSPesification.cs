using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class ActiveLoginMethodSPesification : Specification<LoginMethod>
{
    public ActiveLoginMethodSPesification()
    {
        Query.Where(lm => lm.IsActive && lm.DeletedAt == null);
    }
}
