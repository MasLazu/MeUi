using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Core.Infrastructure.Data.Configurations;

public class PageGroupConfiguration : IEntityTypeConfiguration<PageGroup>
{
    public void Configure(EntityTypeBuilder<PageGroup> builder)
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

        builder.HasMany(a => a.Pages)
            .WithOne(a => a.PageGroup)
            .HasForeignKey(a => a.PageGroupId);
    }
}