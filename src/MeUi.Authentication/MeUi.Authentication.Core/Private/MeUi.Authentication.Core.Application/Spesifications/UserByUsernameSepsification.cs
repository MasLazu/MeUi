using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UserByUsernameSpesification : Specification<User>
{
    public UserByUsernameSpesification(string username)
    {
        Query.Where(u => u.Username == username && u.DeletedAt == null);
    }
}
