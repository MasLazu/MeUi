using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Queries;

public class GetActiveLoginMethodsQuery : ICommand<IEnumerable<LoginMethodDto>> { }