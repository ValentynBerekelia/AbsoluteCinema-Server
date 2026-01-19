using AbsoluteCinema.Domain.Entities;
using CinemaAura.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("ck_permission_has_not_whitespaces", "code NOT LIKE '% %'");
        });

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Id, v => new PermissionId(v))
            .ValueGeneratedNever();

        builder.Property(p => p.Code)
            .HasColumnName("code")
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                code => code.Value,
                value => PermissionCode.Create(value));

        builder.HasIndex(p => p.Code)
            .IsUnique();
    }
}