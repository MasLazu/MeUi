using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class CreateUserCommand : BaseCommand<Guid>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Name { get; set; } = string.Empty;
}