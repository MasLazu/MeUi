using Ardalis.Specification;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RoleByIdSPesification : Specification<Role>
{
    public RoleByIdSPesification(Guid id)
    {
        Query.Where(r => r.Id == id && r.DeletedAt == null);
    }
}
