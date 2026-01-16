using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("actors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new ActorId(value));

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Bio)
            .HasColumnName("bio")
            .IsRequired(false);

        builder.Property(x => x.BirthDate)
            .HasColumnName("birth_date")
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("ix_actors_name");
    }
}