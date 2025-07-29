using System.Reflection;
using MeUi.Authorization.Core.Domain.Entities;
using MeUi.Shared.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeUi.Authorization.Core.Infrastructure.Data;

public class AuthorizationCoreDbContext : BaseDbContext<AuthorizationCoreDbContext>
{
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Domain.Entities.Action> Actions { get; set; }
    public DbSet<ResourceAction> ResourceActions { get; set; }

    public AuthorizationCoreDbContext(DbContextOptions<AuthorizationCoreDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("AuthorizationCore");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}