using FastEndpoints;
using MeUi.Authorization.Core.ApplicationContract.Dtos;

namespace MeUi.Authorization.Core.ApplicationContract.Queries;

public class GetResourcesQuery : ICommand<IEnumerable<ResourceDto>> { }