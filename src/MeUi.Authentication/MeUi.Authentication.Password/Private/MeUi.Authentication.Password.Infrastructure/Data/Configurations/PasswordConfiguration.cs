using MeUi.Authentication.Password.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeUi.Authentication.Password.Infrastructure.Data.Configurations;

public class PasswordConfiguration : IEntityTypeConfiguration<Domain.Entities.Password>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Password> builder)
    {
        builder.HasIndex(u => u.UserLoginMethodId)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        builder.Property(u => u.UserLoginMethodId)
            .IsRequired();
    }
}