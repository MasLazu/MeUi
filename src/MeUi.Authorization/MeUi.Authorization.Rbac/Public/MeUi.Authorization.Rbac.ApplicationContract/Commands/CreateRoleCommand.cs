using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Commands;

public class CreateRoleCommand : BaseCommand<Guid>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}