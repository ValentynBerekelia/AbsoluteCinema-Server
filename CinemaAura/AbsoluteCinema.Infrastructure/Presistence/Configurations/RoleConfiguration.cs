using CinemaAura.Domain.ValueObjects;
using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(x => x.Name);

        builder.OwnsMany<PermissionCode>("_permissions", b =>
        {
            b.ToTable("role_permissions");
            
            builder.Property(r => r.Id)
                .HasConversion(
                    id => id.Id,
                    value => new RoleId(value))
                .ValueGeneratedNever();

            builder.Ignore(r => r.Permissions);
            
            b.WithOwner().HasForeignKey("role_id");
            b.Property<RoleId>("role_id");

            b.Property(p => p.Value)
                .HasColumnName("permission_code")
                .HasMaxLength(128)
                .IsRequired();

            b.HasKey("role_id", nameof(PermissionCode.Value));
            b.HasIndex("role_id", nameof(PermissionCode.Value)).IsUnique();
        });
        
        builder.Navigation("_permissions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}