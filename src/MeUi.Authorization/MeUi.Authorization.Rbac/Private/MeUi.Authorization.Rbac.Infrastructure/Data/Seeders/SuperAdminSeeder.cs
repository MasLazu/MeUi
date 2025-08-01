using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Password.ApplicationContract.Commands;
using MeUi.Authentication.Password.ApplicationContract.Dtos;
using MeUi.Authentication.Password.Shared.Application.Interfaces;
using MeUi.Authorization.Core.ApplicationContract.Dtos;
using MeUi.Authorization.Core.ApplicationContract.Queries;
using MeUi.Authorization.Rbac.Application.Interfaces;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;
using MeUi.Authorization.Rbac.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeUi.Authorization.Rbac.Infrastructure.Data.Seeders;

public class SuperAdminSeeder : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public SuperAdminSeeder(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IConfigurationSection superUserSection = _configuration.GetSection("SuperUser");
        AuthorizationRbacDbContext dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationRbacDbContext>();
        IUserSeeder userSeeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
        IUserLoginMethodSeeder userLoginMethodSeeder = scope.ServiceProvider.GetRequiredService<IUserLoginMethodSeeder>();
        IPasswordSeeder passwordSeeder = scope.ServiceProvider.GetRequiredService<IPasswordSeeder>();

        var user = new UserDto()
        {
            Id = Guid.Parse("01986481-d99a-7f1a-92b3-36bca795ee7d"),
            Username = superUserSection["Username"] ?? "",
            Email = superUserSection["Email"] ?? "",
            Name = superUserSection["Name"] ?? "",
        };
        await userSeeder.SeedAsync(user, ct);

        var userLoginMethod = new UserLoginMethodDto()
        {
            Id = Guid.Parse("0198648d-a37e-75cf-9ff4-f72b0424d6b8"),
            UserId = user.Id,
            LoginMethodCode = "PASSWORD",
        };
        await userLoginMethodSeeder.SeedAsync(userLoginMethod, ct);

        var password = new PasswordDto()
        {
            UserLoginMethodId = userLoginMethod.Id,
            PasswordHash = superUserSection["Password"] ?? "",
        };
        await passwordSeeder.SeedAsync(password, ct);

        Role? superAdminRole = dbContext.Roles.FirstOrDefault(r => r.Code == "SUPER_ADMIN" && r.DeletedAt == null);
        if (superAdminRole == null)
        {
            superAdminRole = new Role()
            {
                Id = Guid.Parse("019863be-7ee2-7627-8c14-6ea1efee3da9"),
                Code = "SUPER_ADMIN",
                Name = "Super Admin",
                Description = "Super Admin Role"
            };
            dbContext.Roles.Add(superAdminRole);
        }

        var getResourceActionsQuery = new GetResourceActionsQuery();
        Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(getResourceActionsQuery.GetType(), typeof(IEnumerable<ResourceActionDto>));
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        dynamic resultTask = handler.ExecuteAsync((dynamic)getResourceActionsQuery, ct);
        var resourceActions = (IEnumerable<ResourceActionDto>)await resultTask;

        IEnumerable<RoleResourceAction> existingRoleresourceActions = dbContext.RoleResourceActions
            .Where(r => r.RoleId == superAdminRole.Id && r.DeletedAt == null);

        IEnumerable<RoleResourceAction> resourceActionidsToAdd = resourceActions
            .Where(ra => !existingRoleresourceActions.Any(era => era.ResourceActionId == ra.Id))
            .Select(ra => new RoleResourceAction()
            {
                RoleId = superAdminRole.Id,
                ResourceActionId = ra.Id,
            });

        IEnumerable<RoleResourceAction> roleResourceActionsToDelete = existingRoleresourceActions
            .Where(era => !resourceActions.Any(ra => ra.Id == era.ResourceActionId));

        dbContext.RoleResourceActions.RemoveRange(roleResourceActionsToDelete);
        dbContext.RoleResourceActions.AddRange(resourceActionidsToAdd);

        await dbContext.SaveChangesAsync(ct);
    }
}
