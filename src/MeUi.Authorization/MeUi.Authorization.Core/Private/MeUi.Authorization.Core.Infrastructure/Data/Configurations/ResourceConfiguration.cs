using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Core.Infrastructure.Data.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
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

        builder.HasMany(r => r.ResourceActions)
            .WithOne(ra => ra.Resource)
            .HasPrincipalKey(r => r.Code)
            .HasForeignKey(ra => ra.ResourceCode);
    }
}