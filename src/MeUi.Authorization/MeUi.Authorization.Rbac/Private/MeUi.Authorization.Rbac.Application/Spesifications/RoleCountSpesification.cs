using Ardalis.Specification;
using MeUi.Authorization.Rbac.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.Spesifications;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class RoleCountSpesification : BasePaginationSpecification<Role>
{
    public RoleCountSpesification(BasePaginationQuery<Role> query)
    {
        Query.Where(r => r.DeletedAt == null);
        ApplyFiltering(query);
    }
}