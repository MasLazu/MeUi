using System.Windows.Input;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Queries;

public class GetUsersByIdsQuery : ICommand<IEnumerable<UserDto>>
{
    public IEnumerable<Guid> Ids { get; set; }
}