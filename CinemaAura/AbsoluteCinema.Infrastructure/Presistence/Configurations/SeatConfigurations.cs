using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class SeatConfigurations : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Id,
                value => new SeatId(value));

        builder.Property(s => s.Number)
            .HasColumnName("number")
            .IsRequired();

        builder.Property(s => s.SeatTypeId)
            .HasColumnName("seat_type_id")
            .HasConversion(
                id => id.Id,
                value => new SeatTypeId(value))
            .IsRequired();
    }
}
