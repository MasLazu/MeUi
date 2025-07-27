using System.Windows.Input;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Queries;

public class GetUserByUsernameQuery : ICommand<UserDto?>
{
    public string Username { get; set; }
}