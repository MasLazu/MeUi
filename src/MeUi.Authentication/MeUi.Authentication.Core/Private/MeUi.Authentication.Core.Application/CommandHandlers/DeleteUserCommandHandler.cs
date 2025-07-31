using Ardalis.Specification;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand, Guid>
{
    private readonly IAuthenticationCoreRepository<User> _userRepository;

    public DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<User> userRepository) : base(unitOfWork)
    {
        _userRepository = userRepository;
    }

    public override async Task<Guid> ExecuteAsync(DeleteUserCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var user = await _userRepository.GetByIdAsync(command.Id, ct);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            _userRepository.Delete(user, ct);

            return user.Id;
        }, ct, _userRepository);
    }
}