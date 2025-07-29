using MeUi.Authentication.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Core.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(lm => lm.Id);

        builder.HasIndex(lm => lm.DeletedAt);

        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.HasIndex(u => u.DeletedAt);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Username)
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .HasMaxLength(255);

        builder.HasMany(u => u.LoginMethods)
            .WithOne(ulm => ulm.User);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User);
    }
}