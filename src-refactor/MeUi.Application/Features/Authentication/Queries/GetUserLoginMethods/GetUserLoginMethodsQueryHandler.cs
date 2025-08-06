using Mapster;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authentication.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authentication.Queries.GetUserLoginMethods;

public class GetUserLoginMethodsQueryHandler : IRequestHandler<GetUserLoginMethodsQuery, IEnumerable<LoginMethodDto>>
{
    private readonly IRepository<UserLoginMethod> _userLoginMethodRepository;
    private readonly IRepository<LoginMethod> _loginMethodRepository;

    public GetUserLoginMethodsQueryHandler(
        IRepository<UserLoginMethod> userLoginMethodRepository,
        IRepository<LoginMethod> loginMethodRepository)
    {
        _userLoginMethodRepository = userLoginMethodRepository;
        _loginMethodRepository = loginMethodRepository;
    }

    public async Task<IEnumerable<LoginMethodDto>> Handle(GetUserLoginMethodsQuery request, CancellationToken ct)
    {
        var userLoginMethods = await _userLoginMethodRepository.GetAllAsync(ct);
        var userLoginMethodCodes = userLoginMethods
            .Where(ulm => ulm.UserId == request.UserId && !ulm.IsDeleted)
            .Select(ulm => ulm.LoginMethodCode)
            .ToList();

        var loginMethods = await _loginMethodRepository.GetAllAsync(ct);
        var userActiveLoginMethods = loginMethods
            .Where(lm => userLoginMethodCodes.Contains(lm.Code) && lm.IsActive && !lm.IsDeleted)
            .ToList();

        return userActiveLoginMethods.Adapt<IEnumerable<LoginMethodDto>>();
    }
}