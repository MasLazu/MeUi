using FastEndpoints;
using MeUi.Authorization.Core.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Queries;

public class GetUserResourceActionsQuery : ICommand<IEnumerable<ResourceActionDto>>
{
    public Guid UserId { get; set; } = Guid.Empty;
}