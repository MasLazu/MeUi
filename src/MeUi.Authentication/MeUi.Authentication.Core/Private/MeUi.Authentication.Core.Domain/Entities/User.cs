using MeUi.Shared.Domain.Entities;

namespace MeUi.Authentication.Core.Domain.Entities;

public class User : BaseEntity
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Name { get; set; }
    public bool IsSuspended { get; set; } = false;

    public ICollection<UserLoginMethod> LoginMethods { get; set; } = new HashSet<UserLoginMethod>();
}