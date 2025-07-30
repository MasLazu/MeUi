using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Core.Domain.Entities;

public class Action : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<ResourceAction> ResourceActions { get; set; } = new HashSet<ResourceAction>();
}