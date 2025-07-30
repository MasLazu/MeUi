using Ardalis.Specification;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RoleByIdWithResourceActionsSpesification : Specification<Role>
{
    public RoleByIdWithResourceActionsSpesification(Guid id)
    {
        Query
            .Include(r => r.RoleResourceActions)
            .Where(r => r.Id == id && r.DeletedAt == null);
    }
}
