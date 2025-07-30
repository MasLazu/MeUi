using MeUi.Shared.Domain.Entities;

namespace MeUi.Authorization.Rbac.Domain.Entities;

public class Role : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<RoleResourceAction> RoleResourceActions { get; set; } = new HashSet<RoleResourceAction>();
    public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}