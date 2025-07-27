using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetUserByEmailQueryHandler : ICommandHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly IReadRepositoryBase<User> _userRepository;

    public GetUserByEmailQueryHandler(IReadRepositoryBase<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> ExecuteAsync(GetUserByEmailQuery command, CancellationToken ct)
    {
        var spesification = new UserByEmailSpesification(command.Email);
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