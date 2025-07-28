using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Password.Infrastructure.Data;
using MeUi.Shared.Domain.Entities;
using MeUi.Shared.Infrastructure.Data.Repositories;

namespace MeUi.Authentication.Core.Infrastructure.Data.Repositories;

public class AuthenticationPasswordRepository<T> : Repository<T>, IAuthenticationPasswordRepository<T> where T : BaseEntity
{
    public AuthenticationPasswordRepository(AuthenticationPasswordDbContext dbContext) : base(dbContext)
    {
    }
}