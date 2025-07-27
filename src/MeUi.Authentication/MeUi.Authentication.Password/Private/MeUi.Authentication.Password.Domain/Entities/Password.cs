using MeUi.Shared.Domain.Entities;

namespace MeUi.Authentication.Password.Domain.Entities;

public class Password : BaseEntity
{
    public Guid UserLoginMethodId { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}