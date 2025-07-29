using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Core.Infrastructure.Data.Configurations;

public class ResourceActionConfiguration : IEntityTypeConfiguration<ResourceAction>
{
    public void Configure(EntityTypeBuilder<ResourceAction> builder)
    {
        builder.HasKey(ra => ra.Id);

        builder.HasIndex(ra => ra.DeletedAt);

        builder.HasIndex(ra => ra.ResourceCode)
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.HasIndex(ra => ra.ActionCode)
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.HasOne(ra => ra.Resource)
            .WithMany(r => r.ResourceActions)
            .HasForeignKey(ra => ra.ResourceCode)
            .HasPrincipalKey(r => r.Code);

        builder.HasOne(ra => ra.Action)
            .WithMany(r => r.ResourceActions)
            .HasForeignKey(ra => ra.ActionCode)
            .HasPrincipalKey(r => r.Code);
    }
}