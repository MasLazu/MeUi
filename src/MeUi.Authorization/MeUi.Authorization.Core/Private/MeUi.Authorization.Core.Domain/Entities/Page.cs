using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Core.Domain.Entities;

public class Page : BaseEntity
{
    public Guid? ParentId { get; set; }
    public Guid? PageGroupId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    public PageGroup? PageGroup { get; set; }
    public ICollection<PageResourceAction> PageResourceActions { get; set; } = new HashSet<PageResourceAction>();
}