using MeUi.Authorization.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authorization.Core.Infrastructure.Data.Configurations;

public class PageResourceActionConfiguration : IEntityTypeConfiguration<PageResourceAction>
{
    public void Configure(EntityTypeBuilder<PageResourceAction> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.DeletedAt);

        builder.HasOne(a => a.Page)
            .WithMany(a => a.PageResourceActions);

        builder.HasOne(a => a.ResourceAction)
            .WithMany(a => a.PageResourceActions);
    }
}