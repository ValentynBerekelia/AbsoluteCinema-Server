using CinemaAura.Infrastructure.Presistence.Configurations.LinkObjects;
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

        builder.OwnsMany<UserRoleLink>("_roleIds", b =>
        {
            b.ToTable("UserRoles");
            
            b.WithOwner().HasForeignKey("UserId");
            b.Property(p => p.UserId);
            b.Property(p => p.RoleId);
            
            b.HasKey(x=> new { x.UserId, x.RoleId });
            b.HasIndex(x => new { x.UserId, x.RoleId });
            
            b.HasOne<Role>()
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        builder.Navigation("_roleIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}