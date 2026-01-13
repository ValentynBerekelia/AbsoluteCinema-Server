using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                value => new ActorId(value));

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Bio)
            .IsRequired();

        builder.Property(x => x.BirthDate)
            .IsRequired();
    }
}