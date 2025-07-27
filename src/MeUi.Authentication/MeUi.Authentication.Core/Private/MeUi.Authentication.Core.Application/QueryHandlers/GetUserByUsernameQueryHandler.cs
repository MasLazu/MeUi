using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetUserByUsernameQueryHandler : ICommandHandler<GetUserByUsernameQuery, UserDto?>
{
    private readonly IReadRepositoryBase<User> _userRepository;

    public GetUserByUsernameQueryHandler(IReadRepositoryBase<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> ExecuteAsync(GetUserByUsernameQuery command, CancellationToken ct)
    {
        var spesification = new UserByUsernameSpesification(command.Username);
        User? user = await _userRepository.FirstOrDefaultAsync(spesification, ct);

        if (user == null)
        {
            return null;
        }

        return new UserDto()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Name = user.Name,
            IsSuspended = user.IsSuspended,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}