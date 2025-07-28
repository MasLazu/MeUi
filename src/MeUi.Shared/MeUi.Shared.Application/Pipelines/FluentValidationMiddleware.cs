// using FastEndpoints;

// namespace MeUi.Shared.Application.Pipelines;

// public sealed class FluentValidationMiddleware<TCommand> : ICommandHandler<TCommand>
//     where TCommand : ICommand
// {
//     public async Task ExecuteAsync(TCommand command, CancellationToken ct)
//     {
//         var validator = Cfg.ServiceResolver.TryResolve<IValidator<TCommand>>();

//         if (validator is not null)
//         {
//             var result = await validator.ValidateAsync(command, ct);

//             if (!result.IsValid)
//                 await result.SendErrorsAsync(ct);
//         }
//     }
// }