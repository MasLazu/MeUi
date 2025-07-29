using Ardalis.Specification;
using MeUi.Authorization.Core.Domain.Entities;

namespace MeUi.Authorization.Core.Application.Spesifications;

public class ResourceActionWithResourceAndActionSpesification : Specification<ResourceAction>
{
    public ResourceActionWithResourceAndActionSpesification()
    {
        Query
            .Include(rs => rs.Resource)
            .Include(rs => rs.Action)
            .Where(rs => rs.DeletedAt == null);
    }
}
