using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Shared.Domain.Entities;
using MeUi.Shared.Infrastructure.Data.Repositories;

namespace MeUi.Authorization.Rbac.Infrastructure.Data.Repositories;

public class AuthorizationRbacRepository<T> : Repository<T>, IAuthorizationRbacRepository<T> where T : BaseEntity
{
    public AuthorizationRbacRepository(AuthorizationRbacDbContext dbContext) : base(dbContext)
    {
    }
}