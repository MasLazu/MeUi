using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authorization.Rbac.ApplicationContract.Dtos;

public class RoleDto : BaseDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public IEnumerable<RoleResourceActionDto> RoleResourceActions { get; set; } = new HashSet<RoleResourceActionDto>();
}