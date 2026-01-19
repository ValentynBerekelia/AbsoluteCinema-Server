using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaAura.Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("movies", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("ck_movies_rate_range", "rate >= 0 AND rate <= 10");
            tableBuilder.HasCheckConstraint("ck_movies_age_limit_positive", "age_limit >= 0");
            tableBuilder.HasCheckConstraint("ck_movies_duration_positive", "duration > 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Id, v => new MovieId(v))
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Rate)
            .HasColumnName("rate")
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(x => x.AgeLimit)
            .HasColumnName("age_limit")
            .IsRequired();

        builder.HasIndex(x => x.Rate)
            .HasDatabaseName("ix_movies_rate");

        builder.Property(x => x.Duration)
            .HasColumnName("duration")
            .HasConversion(
                v => v.TotalSeconds,
                v => TimeSpan.FromSeconds(v))
            .HasColumnType("integer");

        builder.Property(m => m.Studio)
            .HasColumnName("studio")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Language)
            .HasMaxLength(50)
            .HasColumnName("language")
            .IsRequired(false);

        builder.Property(m => m.Country)
            .HasColumnName("country_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Ignore(m => m.PersonIds);
        builder.Ignore(m => m.GenreIds);
        builder.Ignore(m => m.MediaIds);

        builder.OwnsMany<MovieGenre>("_genreIds", b =>
        {
            b.ToTable("movie_genres");

            b.WithOwner().HasForeignKey("movie_id");
            b.Property<MovieId>("movie_id");

            b.Property(p => p.GenreId)
                .HasColumnName("genre_id")
                .HasConversion(
                    id => id.Id,
                    value => new GenreId(value))
                .IsRequired();

            b.HasKey("movie_id", nameof(MovieGenre.GenreId));

            b.HasOne<Genre>()
                .WithMany()
                .HasForeignKey(nameof(MovieGenre.GenreId))
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.OwnsMany<MoviePerson>("_personIds", b =>
        {
            b.ToTable("movie_persons");

            b.WithOwner().HasForeignKey("movie_id");
            b.Property<MovieId>("movie_id");

            b.Property(p => p.PersonId)
                .HasColumnName("actor_id")
                .HasConversion(
                    id => id.Id,
                    value => new PersonId(value))
                .IsRequired();

            b.HasKey("movie_id", nameof(MoviePerson.PersonId));

            b.HasOne<Person>()
                .WithMany()
                .HasForeignKey(nameof(MoviePerson.PersonId))
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.OwnsMany<MovieMedia>("_mediaIds", b =>
        {
            b.ToTable("movie_media");

            b.WithOwner().HasForeignKey("movie_id");
            b.Property<MovieId>("movie_id");
            b.Property(p => p.MediaId)
                .HasColumnName("media_id")
                .HasConversion(
                    id => id.Id,
                    value => new MediaId(value))
                .IsRequired();

            b.HasKey("movie_id", nameof(MovieMedia.MediaId));

            b.HasOne<Media>()
                .WithMany()
                .HasForeignKey(nameof(MovieMedia.MediaId))
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Navigation("_genreIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_personIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_mediaIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
