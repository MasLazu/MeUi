using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Dtos;

public class UserLoginMethodDto : BaseDto
{
    public Guid UserId { get; set; }
    public string LoginMethodCode { get; set; }

    public LoginMethodDto? LoginMethod { get; set; }
}