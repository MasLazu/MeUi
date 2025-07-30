using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Commands;

public class UpdateRoleCommand : BaseCommand<Guid>
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}