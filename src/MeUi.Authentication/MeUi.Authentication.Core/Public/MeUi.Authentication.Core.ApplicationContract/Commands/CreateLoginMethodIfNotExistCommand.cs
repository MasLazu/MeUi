using FastEndpoints;
using MeUi.Shared.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Commands;

public class CreateLoginMethodIfNotExistCommand : BaseCommand<Guid>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}