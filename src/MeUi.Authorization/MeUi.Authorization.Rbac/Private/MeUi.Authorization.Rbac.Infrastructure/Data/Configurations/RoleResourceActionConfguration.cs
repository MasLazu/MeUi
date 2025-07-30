using MeUi.Authorization.Rbac.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Rbac.Infrastructure.Data.Configurations;

public class RoleResourceActionConfiguration : IEntityTypeConfiguration<RoleResourceAction>
{
    public void Configure(EntityTypeBuilder<RoleResourceAction> builder)
    {
        builder.HasKey(ur => ur.Id);

        builder.HasIndex(ur => ur.DeletedAt);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.RoleResourceActions);
    }
}