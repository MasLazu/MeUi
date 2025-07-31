using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Core.Domain.Entities;

public class ResourceAction : BaseEntity
{
    public string ResourceCode { get; set; } = string.Empty;
    public string ActionCode { get; set; } = string.Empty;

    public Resource? Resource { get; set; }
    public Action? Action { get; set; }
    public ICollection<PageResourceAction> PageResourceActions { get; set; } = new HashSet<PageResourceAction>();
}