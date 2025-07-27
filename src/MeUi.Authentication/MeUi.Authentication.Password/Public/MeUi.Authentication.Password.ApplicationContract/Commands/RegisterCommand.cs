using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Password.ApplicationContract.Commands;

public class RegisterCommand : EmptyResultCommand
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
