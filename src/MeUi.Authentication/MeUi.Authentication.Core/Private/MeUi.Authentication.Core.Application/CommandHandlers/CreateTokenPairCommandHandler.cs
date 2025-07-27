using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Application.CommandHandler;
using MeUi.Shared.Application.Exceptions;
using MeUi.Shared.Application.interfaces;

namespace MeUi.Authentication.Core.Application.CommandHandlers;

public class CreateTokenPairCommandHandler : BaseCommandHandler<CreateTokenPairCommand, CreateTokenPairResult>
{
    private readonly IRepository<RefreshToken> _refreshRepository;
    private readonly IJwtService _jwtService;
    private readonly IRepository<User> _userRepository;

    public CreateTokenPairCommandHandler(IRepository<RefreshToken> refreshRepository, IJwtService jwtService, IRepository<User> userRepository)
    {
        _refreshRepository = refreshRepository;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    public override async Task<CreateTokenPairResult> ExecuteAsync(CreateTokenPairCommand command, CancellationToken ct)
    {
        return await WithTransactionAsync(async (ct) =>
        {
            var spesification = new UserByIdSpesification(command.UserId);
            User user = await _userRepository.FirstOrDefaultAsync(spesification, ct)
                ?? throw new NotFoundException(nameof(User));

            RefreshToken refreshToken = _jwtService.GenerateRefreshToken(command.UserId);

            await _refreshRepository.AddAsync(refreshToken, ct);

            return new CreateTokenPairResult()
            {
                AccessTokenExpiresAt = _jwtService.GetAccessTokenExpiration(),
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
            };
        }, command, ct, _refreshRepository);
    }
}