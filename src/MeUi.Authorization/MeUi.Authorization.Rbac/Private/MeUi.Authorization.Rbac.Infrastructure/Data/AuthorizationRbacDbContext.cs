using System.Reflection;
using MeUi.Authorization.Rbac.Domain.Entities;
using MeUi.Shared.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeUi.Authorization.Rbac.Infrastructure.Data;

public class AuthorizationRbacDbContext : BaseDbContext<AuthorizationRbacDbContext>
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RoleResourceAction> RoleResourceActions { get; set; }

    public AuthorizationRbacDbContext(DbContextOptions<AuthorizationRbacDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("AuthorizationRbac");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}