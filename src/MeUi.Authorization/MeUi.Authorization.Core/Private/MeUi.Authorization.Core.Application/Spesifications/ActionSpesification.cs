using Ardalis.Specification;
using MeUi.Authorization.Core.Domain.Entities;

namespace MeUi.Authorization.Core.Application.Spesifications;

public class ActionSpesification : Specification<Domain.Entities.Action>
{
    public ActionSpesification()
    {
        Query.Where(rs => rs.DeletedAt == null);
    }
}
