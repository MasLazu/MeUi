using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Authorization.Core.ApplicationContract.Queries;
using MeUi.Authorization.Core.Domain.Entities;
using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authorization.Core.Application.Spesifications;

namespace MeUi.Authorization.Core.Application.QueryHandlers;

public class GetResourceActionByIdsQueryHandler : ICommandHandler<GetResourceActionByIdsQuery, IEnumerable<ResourceActionDto>>
{
    private readonly IAuthorizationCoreRepository<ResourceAction> _resourceActionRepository;

    public GetResourceActionByIdsQueryHandler(IAuthorizationCoreRepository<ResourceAction> resourceActionRepository)
    {
        _resourceActionRepository = resourceActionRepository;
    }

    public async Task<IEnumerable<ResourceActionDto>> ExecuteAsync(GetResourceActionByIdsQuery command, CancellationToken ct)
    {
        List<ResourceAction> resourceActions = await _resourceActionRepository.ListAsync(new ResourceActionByIdsWithResourceAndActionSpesification(command.Ids), ct);

        return resourceActions.Select(ra => new ResourceActionDto()
        {
            Id = ra.Id,
            ResourceCode = ra.ResourceCode,
            ActionCode = ra.ActionCode,
            CreatedAt = ra.CreatedAt,
            UpdatedAt = ra.UpdatedAt,
            Resource = new ResourceDto()
            {
                Id = ra.Resource!.Id,
                Code = ra.Resource.Code,
                Name = ra.Resource.Name,
                Description = ra.Resource.Description,
                CreatedAt = ra.Resource.CreatedAt,
                UpdatedAt = ra.Resource.UpdatedAt,
            },
            Action = new ActionDto()
            {
                Id = ra.Action!.Id,
                Code = ra.Action.Code,
                Name = ra.Action.Name,
                Description = ra.Action.Description,
                CreatedAt = ra.Action.CreatedAt,
                UpdatedAt = ra.Action.UpdatedAt,
            },
        });
    }
}