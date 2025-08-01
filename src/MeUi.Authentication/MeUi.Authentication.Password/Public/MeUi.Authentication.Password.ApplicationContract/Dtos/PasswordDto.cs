using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authentication.Password.ApplicationContract.Dtos;

public class PasswordDto : BaseDto
{
    public Guid UserLoginMethodId { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? PasswordSalt { get; set; }
}