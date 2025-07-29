using FastEndpoints;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Authorization.Core.ApplicationContract.Queries;
using MeUi.Authorization.Core.Domain.Entities;
using Ardalis.Specification;
using MeUi.Authorization.Core.Application.Spesifications;

namespace MeUi.Authorization.Core.Application.QueryHandlers;

public class GetActionsQueryHandler : ICommandHandler<GetActionsQuery, IEnumerable<ActionDto>>
{
    private readonly IAuthorizationCoreRepository<Domain.Entities.Action> _actionRepository;

    public GetActionsQueryHandler(IAuthorizationCoreRepository<Domain.Entities.Action> actionRepository)
    {
        _actionRepository = actionRepository;
    }

    public async Task<IEnumerable<ActionDto>> ExecuteAsync(GetActionsQuery command, CancellationToken ct)
    {
        List<Domain.Entities.Action> actions = await _actionRepository.ListAsync(new ActionSpesification(), ct);

        return actions.Select(lm => new ActionDto()
        {
            Id = lm.Id,
            Code = lm.Code,
            Name = lm.Name,
            Description = lm.Description,
            CreatedAt = lm.CreatedAt,
            UpdatedAt = lm.UpdatedAt,
        });
    }
}