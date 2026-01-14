using CinemaAura.Domain.Entities;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Persistence.Configurations;

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
            .HasColumnName("user")
            .HasConversion(
                id => id.Id,
                value => new UserId(value))
            .IsRequired();

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
    }
}