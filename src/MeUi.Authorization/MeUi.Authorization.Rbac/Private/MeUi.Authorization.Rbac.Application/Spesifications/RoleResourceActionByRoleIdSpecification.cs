using Ardalis.Specification;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RoleResourceActionByRoleIdSpecification : Specification<RoleResourceAction>
{
    public RoleResourceActionByRoleIdSpecification(Guid roleId)
    {
        Query.Where(r => r.RoleId == roleId && r.DeletedAt == null);
    }
}
