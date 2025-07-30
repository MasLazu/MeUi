using MeUi.Shared.ApplicationContract.Dtos;

namespace MeUi.Authorization.Rbac.ApplicationContract.Dtos;

public class UserRoleDto : BaseDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public RoleDto? Role { get; set; }
}