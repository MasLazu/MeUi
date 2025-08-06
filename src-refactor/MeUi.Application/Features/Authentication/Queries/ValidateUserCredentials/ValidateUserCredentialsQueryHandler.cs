using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authentication.Queries.ValidateUserCredentials;

public class ValidateUserCredentialsQueryHandler : IRequestHandler<ValidateUserCredentialsQuery, bool>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<LoginMethod> _loginMethodRepository;
    private readonly IRepository<UserLoginMethod> _userLoginMethodRepository;
    private readonly IRepository<Password> _passwordRepository;
    private readonly IPasswordHasher _passwordHasher;

    public ValidateUserCredentialsQueryHandler(
        IRepository<User> userRepository,
        IRepository<LoginMethod> loginMethodRepository,
        IRepository<UserLoginMethod> userLoginMethodRepository,
        IRepository<Password> passwordRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _loginMethodRepository = loginMethodRepository;
        _userLoginMethodRepository = userLoginMethodRepository;
        _passwordRepository = passwordRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(ValidateUserCredentialsQuery request, CancellationToken ct)
    {
        // Find user by email
        var users = await _userRepository.GetAllAsync(ct);
        var user = users.FirstOrDefault(u => u.Email == request.Email && !u.IsDeleted);

        if (user == null || user.IsSuspended)
            return false;

        // Find login method by code
        var loginMethods = await _loginMethodRepository.GetAllAsync(ct);
        var loginMethod = loginMethods.FirstOrDefault(lm => lm.Code == request.LoginMethodCode && lm.IsActive && !lm.IsDeleted);

        if (loginMethod == null)
            return false;

        // Find user login method
        var userLoginMethods = await _userLoginMethodRepository.GetAllAsync(ct);
        var userLoginMethod = userLoginMethods.FirstOrDefault(ulm =>
            ulm.UserId == user.Id &&
            ulm.LoginMethodId == loginMethod.Id &&
            !ulm.IsDeleted);

        if (userLoginMethod == null)
            return false;

        // For password authentication, verify the password
        if (request.LoginMethodCode == "PASSWORD")
        {
            var passwords = await _passwordRepository.GetAllAsync(ct);
            var password = passwords.FirstOrDefault(p => p.UserLoginMethodId == userLoginMethod.Id && !p.IsDeleted);

            if (password == null)
                return false;

            return _passwordHasher.VerifyPassword(request.Password, password.PasswordHash);
        }

        return true;
    }
}