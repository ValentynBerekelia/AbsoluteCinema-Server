using CinemaAura.Domain.ValueObjects;
using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Persistence.Configurations;

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

        builder.Ignore(r => r.PermissionsIds);
        builder.Ignore(r => r.UserIds);

        builder.OwnsMany<RolePermission>("_permissionsIds", b =>
        {
            b.ToTable("role_permissions");

            b.WithOwner().HasForeignKey("role_id");
            b.Property<RoleId>("role_id");

            b.Property(p => p.PermissionId)
                .HasColumnName("permission_id")
                .HasConversion(
                    id => id.Id,
                    value => new PermissionId(value))
                .IsRequired();

            b.HasKey("role_id", nameof(RolePermission.PermissionId));

            b.HasOne<Permission>()
                .WithMany()
                .HasForeignKey(nameof(RolePermission.PermissionId))
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Navigation("_permissionsIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}