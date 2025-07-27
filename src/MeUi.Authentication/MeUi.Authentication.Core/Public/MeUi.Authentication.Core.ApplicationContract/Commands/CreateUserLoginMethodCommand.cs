using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class CreateUserLoginMethodCommand : BaseCommand<Guid>
{
    public Guid UserId { get; set; }
    public string LoginMethodCode { get; set; }
}