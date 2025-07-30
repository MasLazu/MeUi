using FastEndpoints;
using MeUi.Authorization.Rbac.ApplicationContract.Dtos;

namespace MeUi.Authorization.Rbac.ApplicationContract.Queries;

public class GetRoleByIdQuery : ICommand<RoleDto>
{
    public Guid Id { get; set; }
}