using Mapster;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Common.Mappings;

public class AuthorizationMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Role, RoleDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<Resource, ResourceDto>()
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<MeUi.Domain.Entities.Action, ActionDto>()
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<Permission, PermissionDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ResourceCode, src => src.ResourceCode)
            .Map(dest => dest.ActionCode, src => src.ActionCode)
            .Map(dest => dest.Resource, src => src.Resource)
            .Map(dest => dest.Action, src => src.Action)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}