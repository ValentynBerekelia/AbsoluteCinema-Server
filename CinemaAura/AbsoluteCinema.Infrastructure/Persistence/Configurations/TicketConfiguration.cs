using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "ck_tickets_status_valid",
                "status IN (0, 1, 2, 3)");
        });
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
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
            .HasColumnName("session_id")
            .HasConversion(
                id => id.Id,
                value => new SessionId(value))
            .IsRequired();

        builder.Property(t => t.SeatId)
            .HasColumnName("seat_id")
            .HasConversion(
                id => id.Id,
                value => new SeatId(value))
            .IsRequired();

        builder.Property(t => t.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasDefaultValue(TicketStatus.Pending);

        builder.Property(t => t.PurchasedAt)
            .HasColumnName("purchased_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // UNIQUE constraint (session_id, seat_id)
        builder.HasIndex(t => new { t.SessionId, t.SeatId })
            .IsUnique()
            .HasDatabaseName("uq_tickets_session_seat");

        // FK
        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(t => t.SessionId)
            .HasConstraintName("fk_tickets_sessions_session_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Seat>()
            .WithMany()
            .HasForeignKey(t => t.SeatId)
            .HasConstraintName("fk_tickets_seats_seat_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .HasConstraintName("fk_tickets_users_user_id")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("ix_tickets_user_id");

        builder.HasIndex(t => t.SessionId)
            .HasDatabaseName("ix_tickets_session_id");

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("ix_tickets_status");

        builder.HasIndex(t => t.PurchasedAt)
            .HasDatabaseName("ix_tickets_purchased_at");
    }
}
