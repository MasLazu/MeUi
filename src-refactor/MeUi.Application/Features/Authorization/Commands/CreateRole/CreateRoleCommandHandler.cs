using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(
        IRepository<Role> roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        // Check if role with the same code already exists
        var existingRole = await _roleRepository.FirstOrDefaultAsync(r => r.Code == request.Code, ct);

        if (existingRole != null)
        {
            throw new InvalidOperationException($"Role with code '{request.Code}' already exists");
        }

        var role = new Role
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description
        };

        await _roleRepository.AddAsync(role, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return role.Id;
    }
}