using CinemaAura.Domain.ValueObjects;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    private IEntityTypeConfiguration<Role> _entityTypeConfigurationImplementation;
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(x => x.Name);

        builder.OwnsMany<PermissionCode>("_permission", b =>
        {
            b.ToTable("RolePermission");

            b.WithOwner().HasForeignKey("RoleId");
            b.Property<Guid>("RoleId");

            b.Property(p => p.Value)
                .HasColumnName("PermissionCode")
                .HasMaxLength(128)
                .IsRequired();

            b.HasKey("RoleId", nameof(PermissionCode.Value));

            b.HasIndex("RoleId", "PermissionCode").IsUnique();
        });
        
        builder.Navigation("_permission")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}