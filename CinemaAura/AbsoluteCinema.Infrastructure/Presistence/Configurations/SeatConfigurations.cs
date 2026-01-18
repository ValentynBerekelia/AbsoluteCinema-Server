using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class SeatConfigurations : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("seats");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new SeatId(value));

        builder.Property(s => s.HallId)
            .HasColumnName("hall_id")
            .HasConversion(
                id => id.Id,
                value => new HallId(value))
            .IsRequired();

        builder.Property(s => s.Row)
            .HasColumnName("row")
            .IsRequired();

        builder.Property(s => s.Number)
            .HasColumnName("number")
            .IsRequired();

        builder.Property(s => s.SeatTypeId)
            .HasColumnName("seat_type_id")
            .HasConversion(
                id => id.Id,
                value => new SeatTypeId(value))
            .IsRequired();

        builder.HasOne<Hall>()
            .WithMany()
            .HasForeignKey(s => s.HallId)
            .HasConstraintName("fk_seats_halls_hall_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SeatType>()
            .WithMany()
            .HasForeignKey(s => s.SeatTypeId)
            .HasConstraintName("fk_seats_seat_types_seat_type_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.HallId, s.Row, s.Number })
            .IsUnique()
            .HasDatabaseName("uq_seats_position");

        builder.HasIndex(s => s.HallId)
            .HasDatabaseName("ix_seats_hall_id");

        builder.HasIndex(s => s.SeatTypeId)
            .HasDatabaseName("ix_seats_seat_type_id");
    }
}
