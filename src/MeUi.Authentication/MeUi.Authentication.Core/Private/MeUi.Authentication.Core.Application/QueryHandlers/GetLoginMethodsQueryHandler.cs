using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetLoginMethodsQueryHandler : ICommandHandler<GetLoginMethodsQuery, IEnumerable<LoginMethodDto>>
{
    private readonly IAuthenticationCoreRepository<LoginMethod> _loginMethodRepository;

    public GetLoginMethodsQueryHandler(IAuthenticationCoreRepository<LoginMethod> loginMethodRepository)
    {
        _loginMethodRepository = loginMethodRepository;
    }

    public async Task<IEnumerable<LoginMethodDto>> ExecuteAsync(GetLoginMethodsQuery command, CancellationToken ct)
    {
        List<LoginMethod> loginMethods = await _loginMethodRepository.ListAsync(ct);

        return loginMethods.Select(lm => new LoginMethodDto()
        {
            Id = lm.Id,
            Code = lm.Code,
            Name = lm.Name,
            Description = lm.Description,
            IsActive = lm.IsActive,
            CreatedAt = lm.CreatedAt,
            UpdatedAt = lm.UpdatedAt,
        });
    }
}