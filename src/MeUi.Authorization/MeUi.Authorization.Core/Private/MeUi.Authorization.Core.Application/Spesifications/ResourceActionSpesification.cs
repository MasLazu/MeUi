using Ardalis.Specification;
using MeUi.Authorization.Core.Domain.Entities;

namespace MeUi.Authorization.Core.Application.Spesifications;

public class ResourceActionSpesification : Specification<ResourceAction>
{
    public ResourceActionSpesification()
    {
        Query.Where(rs => rs.DeletedAt == null);
    }
}
