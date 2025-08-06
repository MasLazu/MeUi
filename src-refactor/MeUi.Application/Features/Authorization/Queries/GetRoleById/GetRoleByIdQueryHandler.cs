using Mapster;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRepository<Role> _roleRepository;

    public GetRoleByIdQueryHandler(IRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id, ct);

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {request.Id} not found");
        }

        return role.Adapt<RoleDto>();
    }
}