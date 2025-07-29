using FastEndpoints;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Authorization.Core.ApplicationContract.Queries;
using MeUi.Authorization.Core.Domain.Entities;
using Ardalis.Specification;
using MeUi.Authorization.Core.Application.Spesifications;

namespace MeUi.Authorization.Core.Application.QueryHandlers;

public class GetResourceQueryHandler : ICommandHandler<GetResourcesQuery, IEnumerable<ResourceDto>>
{
    private readonly IAuthorizationCoreRepository<Resource> _resourceRepository;

    public GetResourceQueryHandler(IAuthorizationCoreRepository<Resource> resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<IEnumerable<ResourceDto>> ExecuteAsync(GetResourcesQuery command, CancellationToken ct)
    {
        List<Resource> resource = await _resourceRepository.ListAsync(new ResourceSpesification(), ct);

        return resource.Select(lm => new ResourceDto()
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