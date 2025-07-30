using FastEndpoints;
using MeUi.Authorization.Core.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Queries;

public class GetResourceActionByIdsQuery : ICommand<IEnumerable<ResourceActionDto>>
{
    public IEnumerable<Guid> Ids { get; set; } = new HashSet<Guid>();
}