using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.Spesifications;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UserCountSpesification : BasePaginationSpecification<User>
{
    public UserCountSpesification(BasePaginationQuery<User> query)
    {
        Query.Where(r => r.DeletedAt == null);
        ApplyFiltering(query);
    }
}