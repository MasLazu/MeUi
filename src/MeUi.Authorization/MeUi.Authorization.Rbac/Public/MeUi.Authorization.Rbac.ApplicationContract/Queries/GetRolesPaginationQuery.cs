using FastEndpoints;
using MeUi.Authorization.Rbac.ApplicationContract.Dtos;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authorization.Rbac.ApplicationContract.Queries;

public class GetRolesPaginationQuery : BasePaginationQuery<RoleDto> { }