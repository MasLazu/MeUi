using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Core.Application.Interfaces;

public interface IAuthorizationCoreRepository<T> : IRepository<T> where T : BaseEntity
{

}