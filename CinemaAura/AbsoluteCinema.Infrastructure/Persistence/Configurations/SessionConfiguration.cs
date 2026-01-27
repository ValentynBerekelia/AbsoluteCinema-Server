using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

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
                value => new SessionId(value))
            .ValueGeneratedNever();

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
        
        builder.Property(s => s.Format)
            .HasColumnName("format")
            .IsRequired();

        builder.HasOne(s => s.Movie)
            .WithMany()
            .HasForeignKey(s => s.MovieId)
            .HasConstraintName("fk_sessions_movies_movie_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Hall)
            .WithMany() 
            .HasForeignKey(s => s.HallId)
            .HasConstraintName("fk_sessions_halls_hall_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Tickets)
            .WithOne(t => t.Session) 
            .HasForeignKey(t => t.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(s => s.Tickets)
            .HasField("_tickets")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);

        builder.HasIndex(s => s.StartDateTime).HasDatabaseName("ix_sessions_start_time");

    }
}
