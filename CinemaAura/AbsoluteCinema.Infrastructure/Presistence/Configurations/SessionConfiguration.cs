using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbsoluteCinema.Domain.Entities;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Id,
                value => new SessionId(value));

        builder.Property(s => s.MovieId)
            .HasColumnName("movie")
            .HasConversion(
                id => id.Id,
                value => new MovieId(value))
            .IsRequired();

        builder.Property(s => s.HallId)
            .HasColumnName("hall")
            .HasConversion(
                id => id.Id,
                value => new HallId(value))
            .IsRequired();

        builder.Property(s => s.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.HasOne<Movie>()
            .WithMany()
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Hall>()
            .WithMany()
            .HasForeignKey(s => s.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(s => s.TicketIds);
    }
}
