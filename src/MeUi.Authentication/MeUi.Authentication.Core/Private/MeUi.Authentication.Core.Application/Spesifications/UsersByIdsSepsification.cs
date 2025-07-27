using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UsersByIdsSpesification : Specification<User>
{
    public UsersByIdsSpesification(IEnumerable<Guid> ids)
    {
        Query.Where(u => ids.Contains(u.Id) && u.DeletedAt == null);
    }
}
