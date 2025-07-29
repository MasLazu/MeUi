using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Dtos;

public class ResourceDto : BaseDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}