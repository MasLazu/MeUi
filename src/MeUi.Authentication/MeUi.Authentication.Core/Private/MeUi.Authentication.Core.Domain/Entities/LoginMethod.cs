using MeUi.Shared.Domain.Entities;

namespace MeUi.Authentication.Core.Domain.Entities;

public class LoginMethod : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<UserLoginMethod> UserLoginMethods { get; set; } = new HashSet<UserLoginMethod>();
}