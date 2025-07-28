using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Application.Spesifications;

namespace MeUi.Authentication.Core.Shared.Infrastructure.Services;

public class LoginMethodSeeder : ILoginMethodSeeder
{
    private readonly IAuthenticationCoreRepository<LoginMethod> _loginMethodRepository;

    public LoginMethodSeeder(IAuthenticationCoreRepository<LoginMethod> loginMethodRepository)
    {
        _loginMethodRepository = loginMethodRepository;
    }

    public async Task SeedAsync(LoginMethodDto loginMethod, CancellationToken ct)
    {
        LoginMethod? existingLoginMethod = await _loginMethodRepository.FirstOrDefaultAsync(new LoginMethodByCodeSepsification(loginMethod.Code), ct);

        if (existingLoginMethod == null)
        {
            await _loginMethodRepository.AddAsync(new LoginMethod()
            {
                Code = loginMethod.Code,
                Name = loginMethod.Name,
                Description = loginMethod.Description,
            });
        }
    }
}