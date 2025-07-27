using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class LoginMethodByCodeSepsification : Specification<LoginMethod>
{
    public LoginMethodByCodeSepsification(string code)
    {
        Query.Where(lm => lm.Code == code && lm.DeletedAt == null);
    }
}
