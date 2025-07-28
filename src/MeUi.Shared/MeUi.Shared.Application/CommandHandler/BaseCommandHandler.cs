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

        IRepository mainRepository = repositories.First();
        bool ownsTransaction = false;
        Transaction = command.Transaction;

        if (Transaction == null)
        {
            Transaction = await mainRepository.BeginTransactionAsync(ct);
            ownsTransaction = true;
        }

        foreach (IRepository repository in repositories)
        {
            if (repository.GetTransaction() == null)
            {
                await repository.UseTransactionAsync(Transaction, ct);
            }
        }

        try
        {
            TResult result = await action(ct);
            if (ownsTransaction)
            {
                await mainRepository.CommitTransactionAsync(ct);
            }
            return result;
        }
        catch
        {
            if (ownsTransaction)
            {
                await mainRepository.RollbbackTransactionAsync(ct);
            }
            throw;
        }
    }

}