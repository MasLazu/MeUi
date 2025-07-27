using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UserByIdSpesification : Specification<User>
{
    public UserByIdSpesification(Guid id)
    {
        Query.Where(u => u.Id == id && u.DeletedAt == null);
    }
}
