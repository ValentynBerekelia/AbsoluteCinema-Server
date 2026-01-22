using AbsoluteCinema.Domain.ValueObjects;
using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

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
        
        builder.HasMany(r => r.Permissions)
            .WithMany() 
            .UsingEntity<Dictionary<string, object>>(
                "role_permissions", 
                j => j.HasOne<Permission>().WithMany().HasForeignKey("permission_id"),
                j => j.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                j => 
                {
                    j.HasKey("role_id", "permission_id"); 
                });

        builder.Navigation(r => r.Permissions)
            .HasField("_permissions") 
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);
    }
}