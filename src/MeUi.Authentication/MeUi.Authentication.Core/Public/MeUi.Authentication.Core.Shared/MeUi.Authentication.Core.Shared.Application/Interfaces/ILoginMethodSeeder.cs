using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.Shared.Application.Interfaces;

public interface ILoginMethodSeeder
{
    Task SeedAsync(LoginMethodDto loginMethod, CancellationToken ct);
}