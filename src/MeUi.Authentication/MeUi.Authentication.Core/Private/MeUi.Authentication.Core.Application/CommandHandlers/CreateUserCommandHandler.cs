using Ardalis.Specification;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand, Guid>
{
    private readonly IRepository<User> _userRepository;

    public CreateUserCommandHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateUserCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var user = new User()
            {
                Username = command.Username,
                Email = command.Email,
                Name = command.Name,
            };

            await _userRepository.AddAsync(user, ct);

            return user.Id;
        }, command, ct, _userRepository);
    }
}