using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAura.Infrastructure.Presistence;

public class CinemaDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Media> Medias => Set<Media>();
    
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CinemaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}