using MeUi.Domain.Common;

namespace MeUi.Domain.Entities;

public class PageResourceAction : BaseEntity
{
    public Guid? PageId { get; set; }
    public Guid? PermissionId { get; set; }

    public Page? Page { get; set; }
    public Permission? Permission { get; set; }
}