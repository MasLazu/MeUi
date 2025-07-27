using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Domain.Entities;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace MeUi.Shared.Infrastructure.Data.Repositories;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : BaseEntity
{
    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public DbTransaction? GetTransaction()
    {
        return _dbContext.Database.CurrentTransaction?.GetDbTransaction();
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken ct)
    {
        return (await _dbContext.Database.BeginTransactionAsync(ct)).GetDbTransaction();
    }

    public async Task UseTransactionAsync(DbTransaction transaction, CancellationToken ct)
    {
        await _dbContext.Database.UseTransactionAsync(transaction, ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct)
    {
        await _dbContext.Database.CommitTransactionAsync(ct);
    }

    public async Task RollbbackTransactionAsync(CancellationToken ct)
    {
        await _dbContext.Database.RollbackTransactionAsync(ct);
    }
}