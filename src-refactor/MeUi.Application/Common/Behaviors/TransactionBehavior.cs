using MediatR;
using MeUi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace MeUi.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IUnitOfWork unitOfWork, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        // Only use transactions for commands (requests that modify data)
        if (!IsCommand(request))
        {
            return await next();
        }

        _logger.LogInformation("Starting transaction for {RequestName}", requestName);

        try
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            var response = await next();

            await _unitOfWork.CommitTransactionAsync(ct);

            _logger.LogInformation("Transaction committed for {RequestName}", requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction failed for {RequestName}, rolling back", requestName);

            try
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
            }
            catch (Exception rollbackEx)
            {
                _logger.LogError(rollbackEx, "Failed to rollback transaction for {RequestName}", requestName);
            }

            throw;
        }
    }

    private static bool IsCommand(TRequest request)
    {
        // Commands typically don't return data or return simple types like Guid, int, bool
        var responseType = typeof(TResponse);

        return responseType == typeof(Unit) || // MediatR Unit type for void commands
               responseType == typeof(Guid) ||
               responseType == typeof(int) ||
               responseType == typeof(bool) ||
               responseType == typeof(string) ||
               request.GetType().Name.EndsWith("Command");
    }
}