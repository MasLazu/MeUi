using MeUi.Authentication.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Core.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(lm => lm.Id);

        builder.HasIndex(lm => lm.DeletedAt);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .IsRequired();

        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        builder.HasIndex(rt => rt.ExpiresAt);
        builder.HasIndex(rt => rt.RevokedAt);

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId);
    }
}