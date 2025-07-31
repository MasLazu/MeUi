using FastEndpoints;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetUserPaginationQueryHandler : ICommandHandler<GetUserPaginationQuery, BasePaginationQueryResult<UserDto>>
{
    private readonly IAuthenticationCoreRepository<User> _userRepository;

    public GetUserPaginationQueryHandler(IAuthenticationCoreRepository<User> roleRepository)
    {
        _userRepository = roleRepository;
    }

    public async Task<BasePaginationQueryResult<UserDto>> ExecuteAsync(GetUserPaginationQuery query, CancellationToken ct)
    {
        var paginationQuery = new BasePaginationQuery<User>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            Filters = query.Filters,
            Sorts = query.Sorts
        };

        var specification = new UserPaginationSpesification(paginationQuery);
        List<User> roles = await _userRepository.ListAsync(specification, ct);

        var countSpecification = new UserCountSpesification(paginationQuery);
        int totalItems = await _userRepository.CountAsync(countSpecification, ct);

        int totalPages = (int)Math.Ceiling((double)totalItems / query.PageSize);

        var users = roles.Select(role => new UserDto
        {
            Id = role.Id,
            Email = role.Email,
            Username = role.Username,
            Name = role.Name,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        }).ToList();

        return new BasePaginationQueryResult<UserDto>
        {
            Items = users,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}