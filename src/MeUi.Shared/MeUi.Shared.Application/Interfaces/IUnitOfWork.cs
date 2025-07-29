namespace MeUi.Shared.Application.interfaces;

public interface IUnitOfWork
{
    int CountRepisotries();
    void AddRepository(IAppRepositoryBase repository);
    void AddRepositories(IAppRepositoryBase[] repositories);
    Task<int> SaveChangesAsync(CancellationToken ct);
}