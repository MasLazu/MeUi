using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Domain.Entities;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
namespace MeUi.Shared.Infrastructure.Data.Repositories;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : BaseEntity
{
    private readonly DbContext _dbContext;
    private bool _ownsTransaction = false;

    public Repository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public T Add(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);
        return entity;
    }

    public IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().AddRange(entities);
        return entities;
    }

    public void Update(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().UpdateRange(entities);
    }

    public void Delete(T entity, CancellationToken cancellationToken = default)
    {
        entity.DeletedAt = DateTimeOffset.UtcNow;
        _dbContext.Set<T>().Update(entity);
    }

    public void DeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (T entity in entities)
        {
            entity.DeletedAt = DateTimeOffset.UtcNow;
        }
        _dbContext.Set<T>().UpdateRange(entities);
    }

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        if (_dbContext.Database.CurrentTransaction == null)
        {
            await _dbContext.Database.BeginTransactionAsync(ct);
            _ownsTransaction = true;
        }
    }

    public async Task CommitTransactionAsync(CancellationToken ct)
    {
        if (_ownsTransaction && _dbContext.Database.CurrentTransaction != null)
        {
            await _dbContext.Database.CommitTransactionAsync(ct);
            _ownsTransaction = false;
        }
    }

    public async Task RollbbackTransactionAsync(CancellationToken ct)
    {
        if (_ownsTransaction && _dbContext.Database.CurrentTransaction != null)
        {
            await _dbContext.Database.RollbackTransactionAsync(ct);
            _ownsTransaction = false;
        }
    }
}