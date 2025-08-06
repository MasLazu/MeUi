using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authentication.Queries.CanUserAuthenticate;

public class CanUserAuthenticateQueryHandler : IRequestHandler<CanUserAuthenticateQuery, bool>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<LoginMethod> _loginMethodRepository;
    private readonly IRepository<UserLoginMethod> _userLoginMethodRepository;

    public CanUserAuthenticateQueryHandler(
        IRepository<User> userRepository,
        IRepository<LoginMethod> loginMethodRepository,
        IRepository<UserLoginMethod> userLoginMethodRepository)
    {
        _userRepository = userRepository;
        _loginMethodRepository = loginMethodRepository;
        _userLoginMethodRepository = userLoginMethodRepository;
    }

    public async Task<bool> Handle(CanUserAuthenticateQuery request, CancellationToken ct)
    {
        // Check if user exists and is active
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null || user.IsSuspended || user.IsDeleted)
            return false;

        // Find login method by code
        var loginMethods = await _loginMethodRepository.GetAllAsync(ct);
        var loginMethod = loginMethods.FirstOrDefault(lm =>
            lm.Code == request.LoginMethodCode &&
            lm.IsActive &&
            !lm.IsDeleted);

        if (loginMethod == null)
            return false;

        // Check if user has this login method configured
        var userLoginMethods = await _userLoginMethodRepository.GetAllAsync(ct);
        var userLoginMethod = userLoginMethods.FirstOrDefault(ulm =>
            ulm.UserId == request.UserId &&
            ulm.LoginMethodCode == request.LoginMethodCode &&
            !ulm.IsDeleted);

        return userLoginMethod != null;
    }
}