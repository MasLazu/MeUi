using FastEndpoints;
using MeUi.Authentication.Core.ApplicationContract.Commands;
using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.Shared.Application.Interfaces;
using MeUi.Authentication.Password.Application.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeUi.Authentication.Password.Infrastructure.Data.seeders;

public class PasswordLoginMethodSeeder : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PasswordLoginMethodSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        ILoginMethodSeeder seeder = scope.ServiceProvider.GetRequiredService<ILoginMethodSeeder>();

        await seeder.SeedAsync(new LoginMethodDto()
        {
            Code = ApplicationConstant.PasswordLoginMethodCode,
            Name = ApplicationConstant.PasswordLoginMethodName,
            Description = ApplicationConstant.PasswordLoginMethodDescription,
        }, ct);
    }
}