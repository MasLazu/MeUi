using MeUi.Shared.Application.Exceptions;

namespace MeUi.Authentication.Password.Application.Exceptions;

public class AccountSuspendedException : ForbiddenException
{
    public AccountSuspendedException() : base("Account suspended") { }
}