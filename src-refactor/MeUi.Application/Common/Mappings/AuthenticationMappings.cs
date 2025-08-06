using Mapster;
using MeUi.Application.Features.Authentication.Commands.Login;
using MeUi.Application.Features.Authentication.Commands.Register;
using MeUi.Application.Features.Authentication.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Common.Mappings;

public class AuthenticationMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // LoginMethod mappings (let Mapster handle automatic mapping)
        config.NewConfig<LoginMethod, LoginMethodDto>();

        // User to UserInfo mapping for token response (let Mapster handle automatic mapping)
        config.NewConfig<User, UserInfo>();

        // Contract to Command mappings (let Mapster handle automatic mapping)
        config.NewConfig<LoginRequest, LoginCommand>();

        // Register command mappings (let Mapster handle automatic mapping)
        config.NewConfig<RegisterCommand, User>();
    }
}