using MeUi.Shared.Application.interfaces;

namespace MeUi.Shared.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly List<IAppRepositoryBase> _repositories = [];

    public void AddRepository(IAppRepositoryBase repository)
    {
        if (!_repositories.Contains(repository))
        {
            _repositories.Add(repository);
        }
    }

    public void AddRepositories(IAppRepositoryBase[] repositories)
    {
        foreach (IAppRepositoryBase repository in repositories)
        {
            if (!_repositories.Contains(repository))
            {
                _repositories.Add(repository);
            }
        }
    }

    public int CountRepisotries()
    {
        return _repositories.Count;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        int entityChangesCount = 0;
        try
        {
            foreach (IAppRepositoryBase repository in _repositories)
            {
                await repository.BeginTransactionAsync(ct);
            }
            foreach (IAppRepositoryBase repository in _repositories)
            {
                entityChangesCount += await repository.SaveChangesAsync(ct);
            }
            foreach (IAppRepositoryBase repository in _repositories)
            {
                await repository.CommitTransactionAsync(ct);
            }
            return entityChangesCount;
        }
        catch (Exception)
        {
            foreach (IAppRepositoryBase repository in _repositories)
            {
                await repository.RollbbackTransactionAsync(ct);
            }
            throw;
        }
    }
}