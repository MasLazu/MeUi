using MeUi.Shared.Domain.Entities;

namespace MeUi.Authentication.Core.Domain.Entities;

public class UserLoginMethod : BaseEntity
{
    public Guid UserId { get; set; }
    public string LoginMethodCode { get; set; }

    public User? User { get; set; }
    public LoginMethod? LoginMethodEntity { get; set; }
}