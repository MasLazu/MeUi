using MeUi.Authentication.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Core.Infrastructure.Data.Configurations;

public class UserLoginMethodConfiguration : IEntityTypeConfiguration<UserLoginMethod>
{
    public void Configure(EntityTypeBuilder<UserLoginMethod> builder)
    {
        builder.HasIndex(ulm => new { ulm.UserId, ulm.LoginMethodCode })
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(ulm => ulm.LoginMethodCode)
            .IsRequired();

        builder.Property(ulm => ulm.UserId)
            .IsRequired();
    }
}