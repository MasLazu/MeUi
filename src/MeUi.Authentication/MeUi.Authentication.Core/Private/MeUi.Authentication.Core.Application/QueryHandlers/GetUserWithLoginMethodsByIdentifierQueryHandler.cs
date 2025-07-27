using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetUserWithLoginMethodsByIdentifierQueryHandler : ICommandHandler<GetUserWithLoginMethodsByIdentifierQuery, UserDto?>
{
    private readonly IReadRepositoryBase<User> _userRepository;

    public GetUserWithLoginMethodsByIdentifierQueryHandler(IReadRepositoryBase<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> ExecuteAsync(GetUserWithLoginMethodsByIdentifierQuery command, CancellationToken ct)
    {
        var spesification = new UserWithLoginMethodsByIdentifierSpesification(command.Identifier);
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
            LoginMethods = user.LoginMethods.Select(lm => new UserLoginMethodDto()
            {
                UserId = lm.UserId,
                LoginMethodCode = lm.LoginMethodCode,
            }).ToList()
        };
    }
}