using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Rbac.Domain.Entities;

public class RoleResourceAction : BaseEntity
{
    public Guid ResourceActionId { get; set; }
    public Guid RoleId { get; set; }

    public Role? Role { get; set; }
}