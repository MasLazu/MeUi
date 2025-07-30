using MeUi.Authorization.Rbac.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Rbac.Infrastructure.Data.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.DeletedAt);

        builder.HasIndex(r => r.Code)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(r => r.RoleResourceActions)
            .WithOne(ra => ra.Role);
    }
}