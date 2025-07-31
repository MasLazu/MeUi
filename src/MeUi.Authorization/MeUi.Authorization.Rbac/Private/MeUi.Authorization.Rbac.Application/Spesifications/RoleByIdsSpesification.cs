using Ardalis.Specification;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RoleByIdsSpesification : Specification<Role>
{
    public RoleByIdsSpesification(IEnumerable<Guid> ids)
    {
        Query.Where(r => ids.Contains(r.Id) && r.DeletedAt == null);
    }
}
