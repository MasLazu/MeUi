using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Rbac.Domain.Entities;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public Role? Role { get; set; }
}