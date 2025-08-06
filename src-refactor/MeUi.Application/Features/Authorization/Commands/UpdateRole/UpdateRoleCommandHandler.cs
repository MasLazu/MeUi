using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Entities;


namespace MeUi.Application.Features.Authorization.Commands.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Guid>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(
        IRepository<Role> roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateRoleCommand request, CancellationToken ct)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id, ct);

        if (role == null)
        {
            throw new InvalidOperationException($"Role with ID '{request.Id}' not found");
        }

        // Check if another role with the same code already exists (if code is being updated)
        if (!string.IsNullOrEmpty(request.Code) && request.Code != role.Code)
        {
            var existingRole = await _roleRepository.FirstOrDefaultAsync(r => r.Code == request.Code && r.Id != request.Id, ct);

            if (existingRole != null)
            {
                throw new InvalidOperationException($"Role with code '{request.Code}' already exists");
            }
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Code))
            role.Code = request.Code;

        if (!string.IsNullOrEmpty(request.Name))
            role.Name = request.Name;

        if (request.Description != null)
            role.Description = request.Description;

        await _roleRepository.UpdateAsync(role, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return role.Id;
    }
}