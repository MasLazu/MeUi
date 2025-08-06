using Mapster;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Application.Common.Models;
using MeUi.Domain.Entities;
using System.Linq.Expressions;

namespace MeUi.Application.Features.Authorization.Queries.GetRolesPaginated;

public class GetRolesPaginatedQueryHandler : IRequestHandler<GetRolesPaginatedQuery, PaginatedResult<RoleDto>>
{
    private readonly IRepository<Role> _roleRepository;

    public GetRolesPaginatedQueryHandler(IRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<PaginatedResult<RoleDto>> Handle(GetRolesPaginatedQuery request, CancellationToken ct)
    {
        Expression<Func<Role, bool>>? predicate = null;

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            predicate = r => r.Name.Contains(request.Search) ||
                           r.Code.Contains(request.Search) ||
                           r.Description.Contains(request.Search);
        }

        // Use the repository's pagination method
        var (roles, totalItems) = await _roleRepository.GetPaginatedAsync(
            predicate: predicate,
            orderBy: r => r.Name,
            skip: (request.Page - 1) * request.PageSize,
            take: request.PageSize,
            ct: ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);

        return new PaginatedResult<RoleDto>
        {
            Items = roles.Adapt<List<RoleDto>>(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}