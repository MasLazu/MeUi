using Ardalis.Specification;
using MeUi.Authorization.Core.Domain.Entities;

namespace MeUi.Authorization.Core.Application.Spesifications;

public class ResourceActionByIdsWithResourceAndActionSpesification : Specification<ResourceAction>
{
    public ResourceActionByIdsWithResourceAndActionSpesification(IEnumerable<Guid> ids)
    {
        Query
            .Include(rs => rs.Resource)
            .Include(rs => rs.Action)
            .Where(ra => ids.Contains(ra.Id) && ra.DeletedAt == null);
    }
}
