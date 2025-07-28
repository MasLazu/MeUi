using Ardalis.Specification;
using FastEndpoints;
using MeUi.Authentication.Core.Application.Interfaces;
using MeUi.Authentication.Core.Application.Spesifications;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Authentication.Core.Domain.Entities;

namespace MeUi.Authentication.Core.Application.QueryHandlers;

public class GetActiveLoginMethodsQueryHandler : ICommandHandler<GetActiveLoginMethodsQuery, IEnumerable<LoginMethodDto>>
{
    private readonly IAuthenticationCoreRepository<LoginMethod> _loginMethodRepository;

    public GetActiveLoginMethodsQueryHandler(IAuthenticationCoreRepository<LoginMethod> loginMethodRepository)
    {
        _loginMethodRepository = loginMethodRepository;
    }

    public async Task<IEnumerable<LoginMethodDto>> ExecuteAsync(GetActiveLoginMethodsQuery command, CancellationToken ct)
    {
        List<LoginMethod> loginMethods = await _loginMethodRepository.ListAsync(new ActiveLoginMethodSPesification(), ct);

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