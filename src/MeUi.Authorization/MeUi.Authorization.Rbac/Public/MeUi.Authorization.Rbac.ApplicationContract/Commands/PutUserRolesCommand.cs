using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Commands;

public class PutUserRolesCommand : BaseCommand<IEnumerable<Guid>>
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> RoleIds { get; set; } = [];
}