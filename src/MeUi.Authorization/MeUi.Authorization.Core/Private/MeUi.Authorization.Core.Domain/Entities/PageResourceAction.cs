using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Core.Domain.Entities;

public class PageResourceAction : BaseEntity
{
    public Guid? PageId { get; set; }
    public Guid? ResourceActionId { get; set; }

    public Page? Page { get; set; }
    public ResourceAction? ResourceAction { get; set; }
}