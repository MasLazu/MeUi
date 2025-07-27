using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Dtos;

public class UserDto : BaseDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Name { get; set; }
    public bool IsSuspended { get; set; } = false;

    public List<UserLoginMethodDto> LoginMethods { get; set; } = [];
}