using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons");

        builder.HasKey(x => x.Id);
        
        builder.Property(p => p.MediaId)
            .HasColumnName("media_id")
            .HasConversion(
                id => id.HasValue ? id.Value.Id : (Guid?)null,
                value => value.HasValue ? new MediaId(value.Value) : null);

        builder.HasOne(p => p.Media)
            .WithMany()
            .HasForeignKey(p => p.MediaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new PersonId(value));

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