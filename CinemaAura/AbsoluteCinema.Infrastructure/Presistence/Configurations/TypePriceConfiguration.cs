using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class TypePriceConfiguration : IEntityTypeConfiguration<TypePrice>
{
    public void Configure(EntityTypeBuilder<TypePrice> builder)
    {
        builder.ToTable("TypePrices");
        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.Id)
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

        builder.HasIndex(tp => new { tp.SessionId, tp.SeatTypeId });
    }
}

