using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetUsersByIdsHandlers : ICommandHandler<GetUsersByIdsQuery, IEnumerable<UserDto>>
{
    private readonly IReadRepositoryBase<User> _userRepository;

    public GetUsersByIdsHandlers(IReadRepositoryBase<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> ExecuteAsync(GetUsersByIdsQuery command, CancellationToken ct)
    {
        var spesification = new UsersByIdsSpesification(command.Ids);
        List<User> users = await _userRepository.ListAsync(spesification, ct);

        return users.Select(u => new UserDto()
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Name = u.Name,
            IsSuspended = u.IsSuspended,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt,
        });
    }
}