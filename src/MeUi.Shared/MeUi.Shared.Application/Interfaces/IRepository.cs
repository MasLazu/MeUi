using System.Data.Common;
using Ardalis.Specification;
using MeUi.Shared.Domain.Entities;

namespace MeUi.Shared.Application.interfaces;

public interface IAppRepositoryBase
{
    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitTransactionAsync(CancellationToken ct);
    Task RollbbackTransactionAsync(CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}

public interface IRepository<T> : IAppRepositoryBase, IReadRepositoryBase<T> where T : BaseEntity
{
    T Add(T entity, CancellationToken cancellationToken);
    IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken cancellationToken);
    void Update(T entity, CancellationToken cancellationToken);
    void UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken);
    void Delete(T entity, CancellationToken cancellationToken);
    void DeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken);
}
