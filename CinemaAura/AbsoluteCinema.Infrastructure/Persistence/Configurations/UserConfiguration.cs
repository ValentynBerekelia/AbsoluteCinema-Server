using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", t =>
        {
            t.HasCheckConstraint(
                "ck_users_username_length",
                "LENGTH(user_name) >= 1");
        });
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasConversion(i => i.Id, value => new UserId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(100)
            .HasDefaultValue("Unknown")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
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

            b.Property(x => x.Salt)
                .HasColumnName("password_salt")
                .HasMaxLength(128)
                .IsRequired();
        });

        builder.Ignore(u => u.DomainEvents);
        
        builder.HasMany(u => u.Roles)
            .WithMany() 
            .UsingEntity<Dictionary<string, object>>(
                "user_roles",
                j => j.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                j => j.HasOne<User>().WithMany().HasForeignKey("user_id"),
                j =>
                {
                    j.Property<Guid>("user_id");
                    j.Property<Guid>("role_id");
                    j.HasKey("user_id", "role_id");
                });

        builder.Navigation(u => u.Roles)
            .HasField("_roles")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);
    }
}