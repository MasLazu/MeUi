using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Domain.Entities;
using MeUi.Shared.Infrastructure.Data.Repositories;

namespace MeUi.Authentication.Core.Infrastructure.Data.Repositories;

public class AuthenticationCoreRepository<T> : Repository<T>, IAuthenticationCoreRepository<T> where T : BaseEntity
{
    public AuthenticationCoreRepository(AuthenticationCoreDbContext dbContext) : base(dbContext)
    {
    }
}