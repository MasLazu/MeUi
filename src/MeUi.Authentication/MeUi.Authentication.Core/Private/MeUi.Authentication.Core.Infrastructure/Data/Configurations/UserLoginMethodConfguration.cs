using MeUi.Authentication.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Core.Infrastructure.Data.Configurations;

public class UserLoginMethodConfiguration : IEntityTypeConfiguration<UserLoginMethod>
{
    public void Configure(EntityTypeBuilder<UserLoginMethod> builder)
    {
        builder.HasKey(lm => lm.Id);

        builder.HasIndex(lm => lm.DeletedAt);

        builder.HasIndex(ulm => new { ulm.UserId, ulm.LoginMethodCode })
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(ulm => ulm.LoginMethodCode)
            .IsRequired();

        builder.Property(ulm => ulm.UserId)
            .IsRequired();

        builder.HasOne(ulm => ulm.User)
            .WithMany(u => u.LoginMethods)
            .HasForeignKey(ulm => ulm.UserId);

        builder.HasOne(ulm => ulm.LoginMethod)
            .WithMany(lm => lm.UserLoginMethods)
            .HasForeignKey(ulm => ulm.LoginMethodCode)
            .HasPrincipalKey(lm => lm.Code);
    }
}