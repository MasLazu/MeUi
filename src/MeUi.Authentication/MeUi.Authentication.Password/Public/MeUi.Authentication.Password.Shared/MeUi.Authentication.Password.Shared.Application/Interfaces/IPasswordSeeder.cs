using MeUi.Authentication.Password.ApplicationContract.Dtos;

namespace MeUi.Authentication.Password.Shared.Application.Interfaces;

public interface IPasswordSeeder
{
    Task SeedAsync(PasswordDto password, CancellationToken ct);
}