using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Shared.Domain.Entities;
using MeUi.Shared.Infrastructure.Data.Repositories;

namespace MeUi.Authorization.Core.Infrastructure.Data.Repositories;

public class AuthorizationCoreRepository<T> : Repository<T>, IAuthorizationCoreRepository<T> where T : BaseEntity
{
    public AuthorizationCoreRepository(AuthorizationCoreDbContext dbContext) : base(dbContext)
    {
    }
}