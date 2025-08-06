using FastEndpoints;
using MeUi.Authorization.Core.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Queries;

public class GetAccessiblePagesQuery : ICommand<IEnumerable<PageGroupDto>>
{
    public Guid UserId { get; set; } = Guid.Empty;
}