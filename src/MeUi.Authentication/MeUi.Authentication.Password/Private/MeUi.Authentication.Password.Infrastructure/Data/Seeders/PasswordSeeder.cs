using FastEndpoints;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Password.Application.Constants;
using MeUi.Authentication.Password.Application.Spesifications;
using MeUi.Authentication.Password.ApplicationContract.Dtos;
using MeUi.Authentication.Password.Domain.Entities;
using MeUi.Authentication.Password.Shared.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeUi.Authentication.Password.Infrastructure.Data.seeders;

public class PasswordSeeder : IPasswordSeeder
{
    private readonly IAuthenticationPasswordRepository<Domain.Entities.Password> _passwordRepository;

    public PasswordSeeder(IAuthenticationPasswordRepository<Domain.Entities.Password> passwordRepository)
    {
        _passwordRepository = passwordRepository;
    }

    public async Task SeedAsync(PasswordDto password, CancellationToken ct)
    {
        Domain.Entities.Password? existingPassword = await _passwordRepository.FirstOrDefaultAsync(new PasswordByUserLoginMethodIdSpesification(password.UserLoginMethodId), ct);

        if (existingPassword == null)
        {
            _passwordRepository.Add(new Domain.Entities.Password()
            {
                UserLoginMethodId = password.UserLoginMethodId,
                PasswordHash = password.PasswordHash,
            }, ct);
            await _passwordRepository.SaveChangesAsync(ct);
        }
    }
}