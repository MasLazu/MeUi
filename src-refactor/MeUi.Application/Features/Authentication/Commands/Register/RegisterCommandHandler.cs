using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authentication.Models;
using MeUi.Domain.Common.Constants;
using MeUi.Domain.Entities;
using RefreshTokenEntity = MeUi.Domain.Entities.RefreshToken;

namespace MeUi.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TokenResponse>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<LoginMethod> _loginMethodRepository;
    private readonly IRepository<UserLoginMethod> _userLoginMethodRepository;
    private readonly IRepository<Password> _passwordRepository;
    private readonly IRepository<RefreshTokenEntity> _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(
        IRepository<User> userRepository,
        IRepository<LoginMethod> loginMethodRepository,
        IRepository<UserLoginMethod> userLoginMethodRepository,
        IRepository<Password> passwordRepository,
        IRepository<RefreshTokenEntity> refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _loginMethodRepository = loginMethodRepository;
        _userLoginMethodRepository = userLoginMethodRepository;
        _passwordRepository = passwordRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TokenResponse> Handle(RegisterCommand request, CancellationToken ct)
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

        // Get password login method
        var passwordLoginMethod = await _loginMethodRepository.FirstOrDefaultAsync(
            lm => lm.Code == AuthenticationConstants.LoginMethods.Password,
            ct);

        if (passwordLoginMethod == null)
        {
            throw new InvalidOperationException("Password authentication method is not available.");
        }

        // Create new user
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Name = request.Name,
            IsSuspended = false
        };

        user = await _userRepository.AddAsync(user, ct);

        // Create user login method
        var userLoginMethod = new UserLoginMethod
        {
            UserId = user.Id,
            LoginMethodCode = AuthenticationConstants.LoginMethods.Password
        };

        userLoginMethod = await _userLoginMethodRepository.AddAsync(userLoginMethod, ct);

        // Hash password and create password record
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var passwordRecord = new Password
        {
            UserLoginMethodId = userLoginMethod.Id,
            PasswordHash = hashedPassword
        };

        await _passwordRepository.AddAsync(passwordRecord, ct);

        // Generate tokens
        var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, ct);
        var refreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(ct);

        // Store refresh token
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7) // 7 days expiration
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // Map user info
        var userInfo = _mapper.Map<UserInfo>(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15), // Access token expires in 15 minutes
            User = userInfo
        };
    }
}