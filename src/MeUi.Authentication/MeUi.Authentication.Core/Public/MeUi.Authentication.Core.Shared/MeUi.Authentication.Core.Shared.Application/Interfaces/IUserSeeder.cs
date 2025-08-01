using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.Shared.Application.Interfaces;

public interface IUserSeeder
{
    Task SeedAsync(UserDto user, CancellationToken ct);
}