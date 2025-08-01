using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Application.Spesifications;

namespace MeUi.Authentication.Core.Infrastructure.Data.Seeders;

public class UserLoginMethodSeeder : IUserLoginMethodSeeder
{
    private readonly IAuthenticationCoreRepository<UserLoginMethod> _userLoginMethodRepository;

    public UserLoginMethodSeeder(IAuthenticationCoreRepository<UserLoginMethod> userLoginMethodRepository)
    {
        _userLoginMethodRepository = userLoginMethodRepository;
    }

    public async Task SeedAsync(UserLoginMethodDto userLoginMethod, CancellationToken ct)
    {
        UserLoginMethod? existingUserLoginMethod = await _userLoginMethodRepository
            .FirstOrDefaultAsync(new UserLoginMethodByUserIdAndLoginMethodCode(userLoginMethod.UserId, userLoginMethod.LoginMethodCode), ct);

        if (existingUserLoginMethod == null)
        {
            _userLoginMethodRepository.Add(new UserLoginMethod()
            {
                Id = userLoginMethod.Id,
                UserId = userLoginMethod.UserId,
                LoginMethodCode = userLoginMethod.LoginMethodCode,
            }, ct);
            await _userLoginMethodRepository.SaveChangesAsync(ct);
        }
    }
}