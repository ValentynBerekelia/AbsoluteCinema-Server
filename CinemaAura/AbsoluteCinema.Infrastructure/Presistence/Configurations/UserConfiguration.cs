using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(u=> u.Id)
            .HasConversion(i => i.Id, value => new UserId(value));
        
        builder.Property(x => x.UserName)
            .HasDefaultValue("Unknown")
            .IsRequired();
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Ignore(r => r.RoleIds);

        builder.OwnsMany<UserRole>("_roleIds", b =>
        {
            b.ToTable("user_roles");

            b.WithOwner().HasForeignKey("user_id");
            b.Property<UserId>("user_id");

            b.Property(x => x.RoleId)
                .HasConversion(id => id.Id, v => new RoleId(v))
                .HasColumnName("role_id")
                .IsRequired();

            b.HasKey("user_id", nameof(UserRole.RoleId));
            builder.OwnsOne(u => u.PasswordHash, b =>
            {
                b.Property(x => x.Value)
                    .HasColumnName("password_hash")
                    .HasMaxLength(512)
                    .IsRequired();
            });

            b.HasOne<Role>()
                .WithMany()
                .HasForeignKey(nameof(UserRole.RoleId))
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        builder.Navigation("_roleIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}