using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbsoluteCinema.Domain.Entities;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("halls", t =>
        {
            t.HasCheckConstraint("ck_halls_vertical_size_positive", "vertical_size > 0");
            t.HasCheckConstraint("ck_halls_horizontal_size_positive", "horizontal_size > 0");
        });

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new HallId(value));

        builder.Property(h => h.HallName)
            .HasColumnName("hall_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(h => h.VerticalSize)
            .HasColumnName("vertical_size")
            .IsRequired();

        builder.Property(h => h.HorizontalSize)
            .HasColumnName("horizontal_size")
            .IsRequired();

        builder.Ignore(h => h.SeatIds);
        builder.Ignore(h => h.SessionIds);
    }
}