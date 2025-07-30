using MeUi.Authorization.Rbac.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Rbac.Infrastructure.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);

        builder.HasIndex(ur => ur.DeletedAt);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles);
    }
}