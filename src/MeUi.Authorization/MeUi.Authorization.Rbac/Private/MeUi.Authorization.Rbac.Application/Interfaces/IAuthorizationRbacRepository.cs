using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Rbac.Application.Interfaces;

public interface IAuthorizationRbacRepository<T> : IRepository<T> where T : BaseEntity
{

}