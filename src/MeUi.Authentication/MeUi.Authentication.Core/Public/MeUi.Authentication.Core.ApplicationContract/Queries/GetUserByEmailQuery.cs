using System.Windows.Input;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Queries;

public class GetUserByEmailQuery : ICommand<UserDto?>
{
    public string Email { get; set; } = string.Empty;
}