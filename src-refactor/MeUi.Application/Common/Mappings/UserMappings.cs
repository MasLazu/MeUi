using Mapster;
using MeUi.Application.Features.Users.Commands.CreateUser;
using MeUi.Application.Features.Users.Commands.UpdateUser;
using MeUi.Application.Features.Users.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Common.Mappings;

public class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity to DTO mappings
        config.NewConfig<User, UserDto>();

        // Request to Entity mappings (let Mapster handle automatic mapping)
        config.NewConfig<CreateUserRequest, User>();
        config.NewConfig<UpdateUserRequest, User>();

        // Command to Entity mappings (let Mapster handle automatic mapping)
        config.NewConfig<CreateUserCommand, User>();

        // Contract to Command mappings (let Mapster handle automatic mapping)
        config.NewConfig<CreateUserRequest, CreateUserCommand>();
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>();
    }
}