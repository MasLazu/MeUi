using FastEndpoints;
using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class CreateLoginMethodIfNotExistCommand : BaseCommand<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}