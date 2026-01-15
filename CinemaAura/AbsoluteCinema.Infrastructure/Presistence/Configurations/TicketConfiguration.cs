using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Id,
                value => new TicketId(value));

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.HasValue ? id.Value.Id : (Guid?)null,
                value => value.HasValue ? new UserId(value.Value) : (UserId?)null)
            .IsRequired(false);

        builder.Property(t => t.SessionId)
            .HasColumnName("session")
            .HasConversion(
                id => id.Id,
                value => new SessionId(value))
            .IsRequired();

        builder.Property(t => t.SeatId)
            .HasColumnName("seat")
            .HasConversion(
                id => id.Id,
                value => new SeatId(value))
            .IsRequired();

        builder.Property(t => t.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(t => t.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Seat>()
            .WithMany()
            .HasForeignKey(t => t.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
