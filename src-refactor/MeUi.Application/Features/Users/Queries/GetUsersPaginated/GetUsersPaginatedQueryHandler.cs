using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Common.Models;
using MeUi.Application.Features.Users.Models;
using MeUi.Domain.Entities;
using System.Linq.Expressions;

namespace MeUi.Application.Features.Users.Queries.GetUsersPaginated;

public class GetUsersPaginatedQueryHandler : IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<UserDto>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public GetUsersPaginatedQueryHandler(
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<UserDto>> Handle(GetUsersPaginatedQuery request, CancellationToken ct)
    {
        // Build filter predicate directly
        Expression<Func<User, bool>>? predicate = null;

        if (!string.IsNullOrEmpty(request.SearchTerm) && request.IsSuspended.HasValue)
        {
            // Both search and suspension filter
            var searchTerm = request.SearchTerm.ToLower();
            var isSuspended = request.IsSuspended.Value;
            predicate = u => u.IsSuspended == isSuspended &&
                           ((u.Name != null && u.Name.ToLower().Contains(searchTerm)) ||
                            (u.Email != null && u.Email.ToLower().Contains(searchTerm)) ||
                            (u.Username != null && u.Username.ToLower().Contains(searchTerm)));
        }
        else if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            // Only search filter
            var searchTerm = request.SearchTerm.ToLower();
            predicate = u => (u.Name != null && u.Name.ToLower().Contains(searchTerm)) ||
                           (u.Email != null && u.Email.ToLower().Contains(searchTerm)) ||
                           (u.Username != null && u.Username.ToLower().Contains(searchTerm));
        }
        else if (request.IsSuspended.HasValue)
        {
            // Only suspension filter
            var isSuspended = request.IsSuspended.Value;
            predicate = u => u.IsSuspended == isSuspended;
        }

        // Use the efficient pagination method
        var (users, totalCount) = await _userRepository.GetPaginatedAsync(
            predicate: predicate,
            orderBy: u => u.Name,
            orderByDescending: false,
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            ct: ct);

        // Map to DTOs
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PaginatedResult<UserDto>
        {
            Items = userDtos,
            Page = request.PageNumber,
            PageSize = request.PageSize,
            TotalItems = totalCount,
            TotalPages = totalPages
        };
    }
}