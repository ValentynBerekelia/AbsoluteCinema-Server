using CinemaAura.Infrastructure.Presistence.Configurations.LinkObjects;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Presistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                value => new MovieId(value));

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.Rate)
            .IsRequired();

        builder.Property(x => x.Age_limit)
            .IsRequired();

        // ===== Movie - Genre =====
        builder.OwnsMany<MovieGenreLink>("_genreIds", b =>
        {
            b.ToTable("MovieGenres");

            b.WithOwner()
                .HasForeignKey("MovieId");

            b.Property(p => p.MovieId);
            b.Property(p => p.GenreId);

            b.HasKey(x => new { x.MovieId, x.GenreId });
            b.HasIndex(x => new { x.MovieId, x.GenreId });
        });

        // ===== Movie - Actor =====
        builder.OwnsMany<MovieActorLink>("_actorIds", b =>
        {
            b.ToTable("MovieActors");

            b.WithOwner()
                .HasForeignKey("MovieId");

            b.Property(p => p.MovieId);
            b.Property(p => p.ActorId);

            b.HasKey(x => new { x.MovieId, x.ActorId });
            b.HasIndex(x => new { x.MovieId, x.ActorId });

            b.HasOne<Actor>()
                .WithMany()
                .HasForeignKey("ActorId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== Movie - Media =====
        builder.OwnsMany<MovieMediaLink>("_mediaIds", b =>
        {
            b.ToTable("MovieMedia");

            b.WithOwner()
                .HasForeignKey("MovieId");

            b.Property(p => p.MovieId);
            b.Property(p => p.MediaId);

            b.HasKey(x => new { x.MovieId, x.MediaId });
            b.HasIndex(x => new { x.MovieId, x.MediaId });

            b.HasOne<Media>()
                .WithMany()
                .HasForeignKey("MediaId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Navigation("_genreIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_actorIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_mediaIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
