using System.Reflection;
using MeUi.Authentication.Password.Domain.Entities;
using MeUi.Shared.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeUi.Authentication.Password.Infrastructure.Data;

public class AuthenticationPasswordDbContext : BaseDbContext<AuthenticationPasswordDbContext>
{
    public DbSet<Domain.Entities.Password> Passwords { get; set; }

    public AuthenticationPasswordDbContext(DbContextOptions<AuthenticationPasswordDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("AuthenticationPassword");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}