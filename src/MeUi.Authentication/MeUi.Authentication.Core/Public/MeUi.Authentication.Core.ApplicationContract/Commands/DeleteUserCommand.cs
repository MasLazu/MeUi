using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class DeleteUserCommand : BaseCommand<Guid>
{
    public Guid Id { get; set; }
}