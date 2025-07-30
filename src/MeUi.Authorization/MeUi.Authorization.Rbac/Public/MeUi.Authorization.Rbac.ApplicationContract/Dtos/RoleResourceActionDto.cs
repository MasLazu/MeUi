using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authorization.Rbac.ApplicationContract.Dtos;

public class RoleResourceActionDto : BaseDto
{
    public Guid ResourceActionId { get; set; }
    public Guid RoleId { get; set; }
    public IEnumerable<ResourceActionDto> ResourceActions { get; set; } = new HashSet<ResourceActionDto>();
}