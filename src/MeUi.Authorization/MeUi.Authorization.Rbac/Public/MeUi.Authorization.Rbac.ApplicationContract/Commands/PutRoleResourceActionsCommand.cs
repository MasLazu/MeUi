using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Commands;

public class PutRoleResourceActionsCommand : BaseCommand<IEnumerable<Guid>>
{
    public Guid RoleId { get; set; }
    public IEnumerable<Guid> ResourceActionIds { get; set; } = [];
}