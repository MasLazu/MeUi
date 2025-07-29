using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Core.Infrastructure.Data.Configurations;

public class ActionConfiguration : IEntityTypeConfiguration<Domain.Entities.Action>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Action> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.DeletedAt);

        builder.HasIndex(a => a.Code)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(a => a.ResourceActions)
            .WithOne(ra => ra.Action)
            .HasPrincipalKey(a => a.Code)
            .HasForeignKey(ra => ra.ActionCode);
    }
}