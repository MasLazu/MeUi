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
    private readonly IAuthenticationCoreRepository<RefreshToken> _refreshRepository;
    private readonly IJwtService _jwtService;
    private readonly IAuthenticationCoreRepository<User> _userRepository;

    public CreateTokenPairCommandHandler(
        IAuthenticationCoreRepository<RefreshToken> refreshRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        IAuthenticationCoreRepository<User> userRepository) : base(unitOfWork)
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

            _refreshRepository.Add(refreshToken, ct);

            return new CreateTokenPairResult()
            {
                AccessTokenExpiresAt = _jwtService.GetAccessTokenExpiration(),
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
            };
        }, ct, _refreshRepository);
    }
}