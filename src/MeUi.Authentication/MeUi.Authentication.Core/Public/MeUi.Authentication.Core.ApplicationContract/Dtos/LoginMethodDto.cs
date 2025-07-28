using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Dtos;

public class LoginMethodDto : BaseDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}