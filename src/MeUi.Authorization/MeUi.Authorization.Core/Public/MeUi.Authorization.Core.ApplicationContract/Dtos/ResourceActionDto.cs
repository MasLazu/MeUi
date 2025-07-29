using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Dtos;

public class ResourceActionDto : BaseDto
{
    public string ResourceCode { get; set; } = string.Empty;
    public string ActionCode { get; set; } = string.Empty;

    public ResourceDto? Resource { get; set; }
    public ActionDto? Action { get; set; }
}