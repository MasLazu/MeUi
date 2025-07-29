using System.Data.Common;
using FastEndpoints;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Shared.Application.CommandHandler;

public abstract class BaseCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : BaseCommand<TResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public BaseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public abstract Task<TResult> ExecuteAsync(TCommand command, CancellationToken ct);

    public async Task<TResult> WithTransactionAsync(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken ct,
        params IAppRepositoryBase[] repositories)
    {
        ct.ThrowIfCancellationRequested();
        bool ownTransaction = _unitOfWork.CountRepisotries() == 0;
        _unitOfWork.AddRepositories(repositories);

        TResult result = await action(ct);
        if (ownTransaction)
        {
            await _unitOfWork.SaveChangesAsync(ct);
        }
        return result;
    }

    public async Task<TResult> WithTransactionAsync(
        Func<CancellationToken, TResult> action,
        CancellationToken ct,
        params IAppRepositoryBase[] repositories)
    {
        ct.ThrowIfCancellationRequested();
        bool ownTransaction = _unitOfWork.CountRepisotries() == 0;
        _unitOfWork.AddRepositories(repositories);

        TResult result = action(ct);
        if (ownTransaction)
        {
            await _unitOfWork.SaveChangesAsync(ct);
        }
        return result;
    }
}