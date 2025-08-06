using Microsoft.EntityFrameworkCore;
using MeUi.Domain.Entities;

namespace MeUi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // User and Authentication entities
    public DbSet<User> Users { get; set; }
    public DbSet<LoginMethod> LoginMethods { get; set; }
    public DbSet<UserLoginMethod> UserLoginMethods { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Password> Passwords { get; set; }

    // Authorization entities
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<MeUi.Domain.Entities.Action> Actions { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    // Page entities
    public DbSet<PageGroup> PageGroups { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<PageResourceAction> PageResourceActions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}