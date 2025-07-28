using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Password.ApplicationContract.Commands;

public class RegisterCommand : EmptyResultCommand
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
