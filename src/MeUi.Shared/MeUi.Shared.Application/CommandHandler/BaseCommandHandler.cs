using System.Data.Common;
using FastEndpoints;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Shared.Application.CommandHandler;

public abstract class BaseCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : BaseCommand<TResult>
{
    public DbTransaction? Transaction;

    public abstract Task<TResult> ExecuteAsync(TCommand command, CancellationToken ct);

    public async Task<TResult> WithTransactionAsync(
        Func<CancellationToken, Task<TResult>> action,
        BaseCommand<TResult> command,
        CancellationToken ct,
        params IRepository[] repositories)
    {
        ct.ThrowIfCancellationRequested();

        IRepository? mainRepository = null;
        bool ownsTransaction = false;
        Transaction = command.Transaction;

        if (Transaction == null)
        {
            foreach (IRepository repository in repositories)
            {
                if (repository.GetTransaction() == null)
                {
                    Transaction = await repository.BeginTransactionAsync(ct);
                    mainRepository = repository;
                    ownsTransaction = true;
                    break;
                }
            }
        }

        foreach (IRepository repository in repositories)
        {
            if (repository.GetTransaction() == null && Transaction != null)
            {
                await repository.UseTransactionAsync(Transaction, ct);
            }
        }

        try
        {
            TResult result = await action(ct);
            if (ownsTransaction && mainRepository != null)
            {
                await mainRepository.CommitTransactionAsync(ct);
            }
            return result;
        }
        catch
        {
            if (ownsTransaction && mainRepository != null)
            {
                await mainRepository.RollbbackTransactionAsync(ct);
            }
            throw;
        }
    }

}