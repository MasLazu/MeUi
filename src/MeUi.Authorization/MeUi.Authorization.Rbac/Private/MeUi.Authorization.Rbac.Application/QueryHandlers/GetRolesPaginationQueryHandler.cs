using FastEndpoints;
using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.Application.Spesifications;
using MeUi.Authorization.Rbac.ApplicationContract.Dtos;
using MeUi.Authorization.Rbac.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authorization.Rbac.Application.QueryHandlers;

public class GetRolesPaginationQueryHandler : ICommandHandler<GetRolesPaginationQuery, BasePaginationQueryResult<RoleDto>>
{
    private readonly IAuthorizationRbacRepository<Role> _roleRepository;

    public GetRolesPaginationQueryHandler(IAuthorizationRbacRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<BasePaginationQueryResult<RoleDto>> ExecuteAsync(GetRolesPaginationQuery query, CancellationToken ct)
    {
        var paginationQuery = new BasePaginationQuery<Role>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            Filters = query.Filters,
            Sorts = query.Sorts
        };

        var specification = new RolePaginationSpesification(paginationQuery);
        List<Role> roles = await _roleRepository.ListAsync(specification, ct);

        var countSpecification = new RoleCountSpesification(paginationQuery);
        int totalItems = await _roleRepository.CountAsync(countSpecification, ct);

        int totalPages = (int)Math.Ceiling((double)totalItems / query.PageSize);

        var roleDtos = roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Code = role.Code,
            Name = role.Name,
            Description = role.Description,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        }).ToList();

        return new BasePaginationQueryResult<RoleDto>
        {
            Items = roleDtos,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}