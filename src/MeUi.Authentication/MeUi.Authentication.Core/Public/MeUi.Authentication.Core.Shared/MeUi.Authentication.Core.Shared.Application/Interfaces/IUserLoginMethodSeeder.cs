using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.Shared.Application.Interfaces;

public interface IUserLoginMethodSeeder
{
    Task SeedAsync(UserLoginMethodDto user, CancellationToken ct);
}