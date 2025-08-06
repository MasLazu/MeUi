using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authentication.Models;
using MeUi.Domain.Common.Constants;
using MeUi.Domain.Entities;
using RefreshTokenEntity = MeUi.Domain.Entities.RefreshToken;

namespace MeUi.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserLoginMethod> _userLoginMethodRepository;
    private readonly IRepository<Password> _passwordRepository;
    private readonly IRepository<RefreshTokenEntity> _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoginCommandHandler(
        IRepository<User> userRepository,
        IRepository<UserLoginMethod> userLoginMethodRepository,
        IRepository<Password> passwordRepository,
        IRepository<RefreshTokenEntity> refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userLoginMethodRepository = userLoginMethodRepository;
        _passwordRepository = passwordRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        // Find user by email or username
        var user = await FindUserAsync(request.EmailOrUsername, ct);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Check if user is suspended
        if (user.IsSuspended)
        {
            throw new UnauthorizedAccessException("User account is suspended.");
        }

        // Find user's password login method
        var userLoginMethod = await _userLoginMethodRepository.FirstOrDefaultAsync(
            ulm => ulm.UserId == user.Id && ulm.LoginMethodCode == AuthenticationConstants.LoginMethods.Password,
            ct);

        if (userLoginMethod == null)
        {
            throw new UnauthorizedAccessException("Password authentication not available for this user.");
        }

        // Get password record
        var passwordRecord = await _passwordRepository.FirstOrDefaultAsync(
            p => p.UserLoginMethodId == userLoginMethod.Id,
            ct);

        if (passwordRecord == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, passwordRecord.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

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

    private async Task<User?> FindUserAsync(string emailOrUsername, CancellationToken ct)
    {
        // Try to find by email first
        if (emailOrUsername.Contains('@'))
        {
            return await _userRepository.FirstOrDefaultAsync(
                u => u.Email == emailOrUsername,
                ct);
        }

        // Try to find by username
        var userByUsername = await _userRepository.FirstOrDefaultAsync(
            u => u.Username == emailOrUsername,
            ct);

        if (userByUsername != null)
        {
            return userByUsername;
        }

        // Fallback: try email even if it doesn't contain @
        return await _userRepository.FirstOrDefaultAsync(
            u => u.Email == emailOrUsername,
            ct);
    }
}