using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Domain.Entities;

namespace MeUi.Authentication.Core.Application.Interfaces;

public interface IAuthenticationPasswordRepository<T> : IRepository<T> where T : BaseEntity
{

}