using Ardalis.Specification;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.Spesifications;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authentication.Core.Application.Spesifications;

public class UserLoginMethodByUserIdAndLoginMethodCode : BasePaginationSpecification<UserLoginMethod>
{
    public UserLoginMethodByUserIdAndLoginMethodCode(Guid userId, string loginMethodCode)
    {
        Query.Where(r => r.UserId == userId && r.LoginMethodCode == loginMethodCode && r.DeletedAt == null);
    }
}