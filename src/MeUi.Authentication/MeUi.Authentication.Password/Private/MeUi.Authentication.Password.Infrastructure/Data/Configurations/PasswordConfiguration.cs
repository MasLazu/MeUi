using MeUi.Authentication.Password.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Password.Infrastructure.Data.Configurations;

public class PasswordConfiguration : IEntityTypeConfiguration<Domain.Entities.Password>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Password> builder)
    {
        builder.HasKey(lm => lm.Id);

        builder.HasIndex(lm => lm.DeletedAt);

        builder.HasIndex(p => p.UserLoginMethodId)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(p => p.UserLoginMethodId)
            .IsRequired();
    }
}