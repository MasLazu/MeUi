using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;
using RefreshTokenEntity = MeUi.Domain.Entities.RefreshToken;

namespace MeUi.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IRepository<RefreshTokenEntity> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(
        IRepository<RefreshTokenEntity> refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return true; // Already logged out
        }

        // Find the refresh token
        var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(
            rt => rt.Token == request.RefreshToken,
            ct);

        if (refreshTokenEntity == null)
        {
            return true; // Token doesn't exist, consider it logged out
        }

        // Revoke the refresh token
        refreshTokenEntity.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(refreshTokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }
}