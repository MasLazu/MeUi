using System.Reflection;
using MeUi.Authentication.Core.Domain.Entities;
using MeUi.Shared.Domain.Entities;
using MeUi.Shared.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeUi.Authentication.Core.Infrastructure.Data;

public class AuthenticationCoreDbContext : BaseDbContext<AuthenticationCoreDbContext>
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserLoginMethod> UserLoginMethods { get; set; }

    public AuthenticationCoreDbContext(DbContextOptions<AuthenticationCoreDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("AuthenticationCore");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}