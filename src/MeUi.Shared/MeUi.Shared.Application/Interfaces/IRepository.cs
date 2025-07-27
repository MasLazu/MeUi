using System.Data.Common;
using Ardalis.Specification;
using MeUi.Shared.Domain.Entities;

namespace MeUi.Shared.Application.interfaces;

public interface IRepository
{
    DbTransaction? GetTransaction();
    Task<DbTransaction> BeginTransactionAsync(CancellationToken ct);
    Task UseTransactionAsync(DbTransaction transaction, CancellationToken ct);
    Task CommitTransactionAsync(CancellationToken ct);
    Task RollbbackTransactionAsync(CancellationToken ct);
}

public interface IRepository<T> : IRepository, IRepositoryBase<T> where T : BaseEntity { }
