using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        // Get existing user
        var user = await _userRepository.GetByIdAsync(request.Id, ct);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{request.Id}' not found.");
        }

        // Check if email is being changed and if new email already exists
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingUserByEmail = await _userRepository.FirstOrDefaultAsync(
                u => u.Email == request.Email && u.Id != request.Id, ct);

            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' already exists.");
            }
        }

        // Check if username is being changed and if new username already exists
        if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username)
        {
            var existingUserByUsername = await _userRepository.FirstOrDefaultAsync(
                u => u.Username == request.Username && u.Id != request.Id, ct);

            if (existingUserByUsername != null)
            {
                throw new InvalidOperationException($"User with username '{request.Username}' already exists.");
            }
        }

        // Update user properties
        user.Username = request.Username;
        user.Email = request.Email;
        user.Name = request.Name;
        user.IsSuspended = request.IsSuspended;

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return user.Id;
    }
}