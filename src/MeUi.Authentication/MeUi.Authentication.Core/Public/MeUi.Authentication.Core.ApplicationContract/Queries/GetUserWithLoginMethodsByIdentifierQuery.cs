using System.Windows.Input;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Queries;

public class GetUserWithLoginMethodsByIdentifierQuery : ICommand<UserDto?>
{
    public string Identifier { get; set; } = string.Empty;
}