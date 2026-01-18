using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class TypePriceConfiguration : IEntityTypeConfiguration<TypePrice>
{
    public void Configure(EntityTypeBuilder<TypePrice> builder)
    {
        builder.ToTable("type_prices", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("ck_type_prices_price_positive", "price >= 0");
        });

        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new TypePriceId(value));

        builder.Property(tp => tp.SessionId)
            .HasColumnName("session_id")
            .HasConversion(
                id => id.Id,
                value => new SessionId(value))
            .IsRequired();

        builder.Property(tp => tp.SeatTypeId)
            .HasColumnName("seat_type_id")
            .HasConversion(
                id => id.Id,
                value => new SeatTypeId(value))
            .IsRequired();

        builder.Property(tp => tp.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(tp => new { tp.SessionId, tp.SeatTypeId })
            .IsUnique()
            .HasDatabaseName("uq_type_prices_session_seat_type");

        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(tp => tp.SessionId)
            .HasConstraintName("fk_type_prices_sessions_session_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SeatType>()
            .WithMany()
            .HasForeignKey(tp => tp.SeatTypeId)
            .HasConstraintName("fk_type_prices_seat_types_seat_type_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tp => tp.SessionId)
            .HasDatabaseName("ix_type_prices_session_id");

        builder.HasIndex(tp => tp.SeatTypeId)
            .HasDatabaseName("ix_type_prices_seat_type_id");
    }
}

