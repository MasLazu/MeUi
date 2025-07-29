using FastEndpoints;
using MeUi.Authorization.Core.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Queries;

public class GetResourceActionsQuery : ICommand<IEnumerable<ResourceActionDto>> { }