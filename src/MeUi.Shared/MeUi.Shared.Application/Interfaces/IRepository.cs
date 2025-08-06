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
    T Add(T entity, CancellationToken ct);
    IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken ct);
    void Update(T entity, CancellationToken ct);
    void UpdateRange(IEnumerable<T> entities, CancellationToken ct);
    void Delete(T entity, CancellationToken ct);
    void DeleteRange(IEnumerable<T> entities, CancellationToken ct);
}
