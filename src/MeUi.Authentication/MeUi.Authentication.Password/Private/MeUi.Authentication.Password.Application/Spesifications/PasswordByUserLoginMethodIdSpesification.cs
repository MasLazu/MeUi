using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MeUi.Authentication.Password.Application.Spesifications;

public class PasswordByUserLoginMethodIdSpesification : Specification<Domain.Entities.Password>
{
    public PasswordByUserLoginMethodIdSpesification(Guid id)
    {
        Query.Where(lm => lm.UserLoginMethodId == id && lm.DeletedAt == null);
    }
}