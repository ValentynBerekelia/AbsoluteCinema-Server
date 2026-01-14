using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class SeatTypeConfiguration : IEntityTypeConfiguration<SeatType>
{
    public void Configure(EntityTypeBuilder<SeatType> builder)
    {
        builder.ToTable("SeatTypes");
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Id)
            .HasConversion(
                id => id.Id,
                value => new SeatTypeId(value));

        builder.Property(st => st.TypeName)
            .HasColumnName("type_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(st => st.TypeName);
    }
}

