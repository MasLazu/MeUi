using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Dtos;

public class LoginMethodDto : BaseDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;
}