using Ardalis.Specification;
using MeUi.Authorization.Rbac.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RolePaginationSpesification : Specification<Role>
{
    public RolePaginationSpesification(GetRolesPaginationQuery query)
    {
        Query
            .AsNoTracking()
            .AsSplitQuery()
            .Skip(query.Page * query.PageSize)
            .Take(query.PageSize)
            .Include(r => r.RoleResourceActions)
            .Where(r => r.DeletedAt == null);
    }
}
