using Ardalis.Specification;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RoleByCodeSPesification : Specification<Role>
{
    public RoleByCodeSPesification(string code)
    {
        Query.Where(r => r.Code == code && r.DeletedAt == null);
    }
}
