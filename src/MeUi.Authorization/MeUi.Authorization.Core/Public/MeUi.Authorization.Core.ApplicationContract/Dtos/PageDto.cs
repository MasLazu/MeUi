using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Dtos;

public class PageDto : BaseDto
{
    public Guid? ParentId { get; set; }
    public Guid? PageGroupId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}