using FastEndpoints;
using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Authorization.Core.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Dtos;
using MeUi.Authorization.Rbac.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Application.Exceptions;

namespace MeUi.Authorization.Rbac.Application.QueryHandlers;

public class GetRoleByIdQueryHandler : ICommandHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IAuthorizationRbacRepository<Role> _roleRepository;

    public GetRoleByIdQueryHandler(
        IAuthorizationRbacRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> ExecuteAsync(GetRoleByIdQuery command, CancellationToken ct)
    {
        Role role = await _roleRepository.FirstOrDefaultAsync(new RoleByIdSPesification(command.Id), ct) ??
            throw new NotFoundException("Role not found");

        IEnumerable<Guid> resourceActionIds = role.RoleResourceActions.Select(rra => rra.ResourceActionId).Distinct();
        IEnumerable<ResourceActionDto> resourceActions = new HashSet<ResourceActionDto>();
        if (resourceActionIds != null && resourceActionIds.Any())
        {
            resourceActions = await new GetResourceActionByIdsQuery() { Ids = resourceActionIds }.ExecuteAsync();
        }

        return new RoleDto()
        {
            Id = role.Id,
            Code = role.Code,
            Name = role.Name,
            Description = role.Description,
            RoleResourceActions = role.RoleResourceActions.Select(r => new RoleResourceActionDto()
            {
                ResourceActionId = r.ResourceActionId,
                RoleId = r.RoleId,
                ResourceActions = resourceActions.Where(ra => ra.Id == r.ResourceActionId)
            }),
        };
    }
}