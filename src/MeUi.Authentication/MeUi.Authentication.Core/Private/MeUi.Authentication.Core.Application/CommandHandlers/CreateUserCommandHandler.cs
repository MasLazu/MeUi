using Ardalis.Specification;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand, Guid>
{
    private readonly IAuthenticationCoreRepository<User> _userRepository;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<User> userRepository) : base(unitOfWork)
    {
        _userRepository = userRepository;
    }

    public override async Task<Guid> ExecuteAsync(CreateUserCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync((ct) =>
        {
            var user = new User()
            {
                Username = command.Username,
                Email = command.Email,
                Name = command.Name,
            };

            _userRepository.Add(user, ct);

            return user.Id;
        }, ct, _userRepository);
    }
}