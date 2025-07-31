using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Core.Infrastructure.Data.Configurations;

public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
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

        builder.HasOne(a => a.PageGroup)
            .WithMany(a => a.Pages)
            .HasForeignKey(a => a.PageGroupId);

        builder.HasMany(a => a.PageResourceActions)
            .WithOne(a => a.Page)
            .HasForeignKey(a => a.PageId);
    }
}