using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // mb add check constrint for email validation later?
        builder.ToTable("users", t =>
        {
            t.HasCheckConstraint(
                "ck_users_username_length",
                "LENGTH(user_name) >= 1"); // mb also add username validation constraints
        });
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasConversion(i => i.Id, value => new UserId(value));

        builder.Property(x => x.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(100)
            .HasDefaultValue("Unknown")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(255)  // RFC 5321 standard
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .HasDatabaseName("ix_users_email")
            .IsUnique();

        builder.OwnsOne(u => u.PasswordHash, b =>
        {
            b.Property(x => x.Value)
                .HasColumnName("password_hash")
                .HasMaxLength(512)
                .IsRequired();

            b.Property(x => x.Salt).HasColumnName("password_salt")
                .HasMaxLength(128)
                .IsRequired()
                ;
        });

        builder.Ignore(r => r.RoleIds);
        builder.Ignore(u => u.DomainEvents);

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


            b.HasOne<Role>()
                .WithMany()
                .HasForeignKey(nameof(UserRole.RoleId))
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Navigation("_roleIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}