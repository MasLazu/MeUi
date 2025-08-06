using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // Check if user with email already exists
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingUserByEmail = await _userRepository.FirstOrDefaultAsync(
                u => u.Email == request.Email, ct);

            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' already exists.");
            }
        }

        // Check if user with username already exists
        if (!string.IsNullOrEmpty(request.Username))
        {
            var existingUserByUsername = await _userRepository.FirstOrDefaultAsync(
                u => u.Username == request.Username, ct);

            if (existingUserByUsername != null)
            {
                throw new InvalidOperationException($"User with username '{request.Username}' already exists.");
            }
        }

        // Create new user
        var user = _mapper.Map<User>(request);
        user = await _userRepository.AddAsync(user, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        return user.Id;
    }
}