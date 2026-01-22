using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new HallId(value))
            .ValueGeneratedNever();;

        builder.Property(h => h.HallName)
            .HasColumnName("hall_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Ignore(h => h.SeatIds);
        builder.Ignore(h => h.SessionIds);
    }
}