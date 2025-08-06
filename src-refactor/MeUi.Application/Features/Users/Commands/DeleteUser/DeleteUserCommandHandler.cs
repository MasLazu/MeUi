using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Guid>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        // Get existing user
        var user = await _userRepository.GetByIdAsync(request.Id, ct);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{request.Id}' not found.");
        }

        // Soft delete the user
        await _userRepository.DeleteAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return user.Id;
    }
}