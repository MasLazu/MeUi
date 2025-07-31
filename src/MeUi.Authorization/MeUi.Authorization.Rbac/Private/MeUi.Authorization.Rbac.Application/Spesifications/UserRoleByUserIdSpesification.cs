using Ardalis.Specification;
using MeUi.Authorization.Rbac.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Spesifications;

public class UserRoleByUserIdSpesification : Specification<UserRole>
{
    public UserRoleByUserIdSpesification(Guid userId)
    {
        Query.Where(r => r.UserId == userId && r.DeletedAt == null);
    }
}
