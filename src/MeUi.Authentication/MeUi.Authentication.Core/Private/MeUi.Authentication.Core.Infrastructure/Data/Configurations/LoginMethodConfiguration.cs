using MeUi.Authentication.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Core.Infrastructure.Data.Configurations;

public class LoginMethodConfiguration : IEntityTypeConfiguration<LoginMethod>
{
    public void Configure(EntityTypeBuilder<LoginMethod> builder)
    {
        builder.HasKey(lm => lm.Id);

        builder.HasIndex(lm => lm.DeletedAt);

        builder.HasIndex(lm => lm.Code)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(lm => lm.Code)
            .IsRequired();

        builder.Property(lm => lm.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(lm => lm.Code)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(lm => lm.UserLoginMethods)
            .WithOne(ulm => ulm.LoginMethod)
            .HasPrincipalKey(lm => lm.Code)
            .HasForeignKey(ulm => ulm.LoginMethodCode);
    }
}