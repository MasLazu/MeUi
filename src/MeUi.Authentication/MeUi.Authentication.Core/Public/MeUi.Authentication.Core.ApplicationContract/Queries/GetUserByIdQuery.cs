using System.Windows.Input;
using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;

namespace MeUi.Authentication.Core.ApplicationContract.Queries;

public class GetUserByIdQuery : ICommand<UserDto?>
{
    public Guid Id { get; set; }
}