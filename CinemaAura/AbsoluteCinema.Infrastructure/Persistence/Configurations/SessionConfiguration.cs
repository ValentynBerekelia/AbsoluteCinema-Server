using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbsoluteCinema.Domain.Entities;

namespace CinemaAura.Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Id,
                value => new SessionId(value));

        builder.Property(s => s.MovieId)
            .HasColumnName("movie_id")
            .HasConversion(
                id => id.Id,
                value => new MovieId(value))
            .IsRequired();

        builder.Property(s => s.HallId)
            .HasColumnName("hall_id")
            .HasConversion(
                id => id.Id,
                value => new HallId(value))
            .IsRequired();

        builder.Property(s => s.StartDateTime)
            .HasColumnName("start_time")
            .IsRequired();

        builder.HasOne<Movie>()
            .WithMany()
            .HasForeignKey(s => s.MovieId)
            .HasConstraintName("fk_sessions_movies_movie_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Hall>()
            .WithMany()
            .HasForeignKey(s => s.HallId)
            .HasConstraintName("fk_sessions_halls_hall_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.MovieId)
            .HasDatabaseName("ix_sessions_movie_id");

        builder.HasIndex(s => s.HallId)
            .HasDatabaseName("ix_sessions_hall_id");

        builder.HasIndex(s => s.StartDateTime)
            .HasDatabaseName("ix_sessions_start_time");

        builder.Ignore(s => s.TicketIds);
    }
}
