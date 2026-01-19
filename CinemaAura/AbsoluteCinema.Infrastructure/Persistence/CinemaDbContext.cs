using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAura.Infrastructure.Persistence;

public class CinemaDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Media> Medias => Set<Media>();
    public DbSet<Hall> Halls => Set<Hall>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<SeatType> SeatTypes => Set<SeatType>();
    public DbSet<TypePrice> TypePrices => Set<TypePrice>();
    public DbSet<Permission> Permissions => Set<Permission>();
    
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CinemaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}