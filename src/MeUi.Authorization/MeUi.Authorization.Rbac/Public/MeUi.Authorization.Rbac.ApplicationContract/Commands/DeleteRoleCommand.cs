using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Commands;

public class DeleteRoleCommand : BaseCommand<Guid>
{
    public Guid Id { get; set; }
}