using CinemaAura.Domain.ValueObjects;
using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles")
            .HasKey(x => x.Id);

        builder.Property(r => r.Id)
           .HasColumnName("id")
           .HasConversion(
               id => id.Id,
               value => new RoleId(value));

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("uq_roles_name");

        builder.Ignore(r => r.Permissions);
        builder.Ignore(r => r.UserIds);

        builder.OwnsMany<PermissionCode>("_permissions", b =>
        {
            b.ToTable("role_permissions");

            b.WithOwner().HasForeignKey("role_id");

            b.Property<RoleId>("role_id")
                .HasConversion(i => i.Id, v => new RoleId(v));

            b.Property(p => p.Value)
                .HasColumnName("permission_code")
                .HasMaxLength(128)
                .IsRequired();

            b.HasKey("role_id", nameof(PermissionCode.Value));

            b.HasIndex("role_id", nameof(PermissionCode.Value))
                .IsUnique()
                .HasDatabaseName("uq_role_permissions_role_permission");
        });

        builder.Navigation("_permissions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}