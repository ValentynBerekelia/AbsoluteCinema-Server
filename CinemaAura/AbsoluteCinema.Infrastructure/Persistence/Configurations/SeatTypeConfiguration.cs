using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

public class SeatTypeConfiguration : IEntityTypeConfiguration<SeatType>
{
    public void Configure(EntityTypeBuilder<SeatType> builder)
    {
        builder.ToTable("seat_types");
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new SeatTypeId(value));

        builder.Property(st => st.TypeName)
            .HasColumnName("type_name")
            .HasMaxLength(100)
            .IsRequired();

        // UNIQUE constraint
        builder.HasIndex(st => st.TypeName)
            .IsUnique()
            .HasDatabaseName("uq_seat_types_type_name");
    }
}

