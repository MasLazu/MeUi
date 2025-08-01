using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Application.Spesifications;

namespace MeUi.Authentication.Core.Infrastructure.Data.Seeders;

public class UserSeeder : IUserSeeder
{
    private readonly IAuthenticationCoreRepository<User> _userRepository;

    public UserSeeder(IAuthenticationCoreRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task SeedAsync(UserDto user, CancellationToken ct)
    {
        User? existingUser = await _userRepository.FirstOrDefaultAsync(new UserByIdSpesification(user.Id), ct);

        if (existingUser == null)
        {
            _userRepository.Add(new User()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
            }, ct);
            await _userRepository.SaveChangesAsync(ct);
        }
    }
}