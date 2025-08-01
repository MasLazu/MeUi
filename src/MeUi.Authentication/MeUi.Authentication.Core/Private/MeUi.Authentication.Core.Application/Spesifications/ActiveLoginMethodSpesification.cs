using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class ActiveLoginMethodSpesification : Specification<LoginMethod>
{
    public ActiveLoginMethodSpesification()
    {
        Query.Where(lm => lm.IsActive && lm.DeletedAt == null);
    }
}
