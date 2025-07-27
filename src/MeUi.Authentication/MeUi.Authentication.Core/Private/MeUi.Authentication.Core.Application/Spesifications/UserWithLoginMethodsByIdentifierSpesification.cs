using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UserWithLoginMethodsByIdentifierSpesification : Specification<User>
{
    public UserWithLoginMethodsByIdentifierSpesification(string Identifier)
    {
        Query
            .Include(u => u.LoginMethods)
            .Where(u => u.Email == Identifier || u.Username == Identifier && u.DeletedAt == null);
    }
}
