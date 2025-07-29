using Ardalis.Specification;
using MeUi.Authorization.Core.Domain.Entities;

namespace MeUi.Authorization.Core.Application.Spesifications;

public class ResourceSpesification : Specification<Resource>
{
    public ResourceSpesification()
    {
        Query.Where(rs => rs.DeletedAt == null);
    }
}
