using Mapster;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetPermissions;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<Domain.Entities.Action> _actionRepository;

    public GetPermissionsQueryHandler(
        IRepository<Permission> permissionRepository,
        IRepository<Resource> resourceRepository,
        IRepository<Domain.Entities.Action> actionRepository)
    {
        _permissionRepository = permissionRepository;
        _resourceRepository = resourceRepository;
        _actionRepository = actionRepository;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken ct)
    {
        var permissions = await _permissionRepository.GetAllAsync(ct);
        var resources = await _resourceRepository.GetAllAsync(ct);
        var actions = await _actionRepository.GetAllAsync(ct);

        var permissionDtos = permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            ResourceId = resources.FirstOrDefault(r => r.Code == p.ResourceCode)?.Id ?? Guid.Empty,
            ActionId = actions.FirstOrDefault(a => a.Code == p.ActionCode)?.Id ?? Guid.Empty,
            ResourceCode = p.ResourceCode,
            ActionCode = p.ActionCode,
            Resource = resources.FirstOrDefault(r => r.Code == p.ResourceCode)?.Adapt<ResourceDto>(),
            Action = actions.FirstOrDefault(a => a.Code == p.ActionCode)?.Adapt<ActionDto>(),
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).OrderBy(p => p.Resource?.Name).ThenBy(p => p.Action?.Name);

        return permissionDtos;
    }
}