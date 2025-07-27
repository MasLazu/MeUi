using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UserByEmailSpesification : Specification<User>
{
    public UserByEmailSpesification(string email)
    {
        Query.Where(u => u.Email == email && u.DeletedAt == null);
    }
}
