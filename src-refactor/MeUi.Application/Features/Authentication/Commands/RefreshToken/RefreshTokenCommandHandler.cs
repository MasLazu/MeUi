using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authentication.Models;
using MeUi.Domain.Entities;
using RefreshTokenEntity = MeUi.Domain.Entities.RefreshToken;

namespace MeUi.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly IRepository<RefreshTokenEntity> _refreshTokenRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RefreshTokenCommandHandler(
        IRepository<RefreshTokenEntity> refreshTokenRepository,
        IRepository<User> userRepository,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        // Find the refresh token
        var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(
            rt => rt.Token == request.RefreshToken,
            ct);

        if (refreshTokenEntity == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        // Check if token is expired
        if (refreshTokenEntity.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token has expired.");
        }

        // Check if token is revoked
        if (refreshTokenEntity.RevokedAt.HasValue)
        {
            throw new UnauthorizedAccessException("Refresh token has been revoked.");
        }

        // Get the user
        var user = await _userRepository.GetByIdAsync(refreshTokenEntity.UserId, ct);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        // Check if user is suspended
        if (user.IsSuspended)
        {
            throw new UnauthorizedAccessException("User account is suspended.");
        }

        // Generate new tokens
        var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, ct);
        var newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(ct);

        // Revoke the old refresh token
        refreshTokenEntity.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(refreshTokenEntity, ct);

        // Create new refresh token
        var newRefreshTokenEntity = new RefreshTokenEntity
        {
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7) // 7 days expiration
        };

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // Map user info
        var userInfo = _mapper.Map<UserInfo>(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15), // Access token expires in 15 minutes
            User = userInfo
        };
    }
}