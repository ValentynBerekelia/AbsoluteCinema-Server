using AbsoluteCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsoluteCinema.Infrastructure.Persistence.Configurations;

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

        
        builder.HasMany(m => m.Genres)
            .WithMany() 
            .UsingEntity<Dictionary<string, object>>(
                "movie_genres", 
                j => j.HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("genre_id"),
                j => j.HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey("movie_id"),
                j => 
                {
                    j.HasKey("movie_id", "genre_id");
                });
        
        builder.HasMany(m => m.Persons)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "movie_persons",
                j => j.HasOne<Person>().WithMany().HasForeignKey("person_id"),
                j => j.HasOne<Movie>().WithMany().HasForeignKey("movie_id")
            );
        
        builder.HasMany(m => m.Medias)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "movie_media",
                j => j.HasOne<Media>().WithMany().HasForeignKey("media_id"),
                j => j.HasOne<Movie>().WithMany().HasForeignKey("movie_id")
            );

        builder.Navigation(m => m.Genres)
            .HasField("_genres")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);

        builder.Navigation(m => m.Persons)
            .HasField("_persons")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);

        builder.Navigation(m => m.Medias)
            .HasField("_medias")
            .UsePropertyAccessMode(PropertyAccessMode.PreferField);
    }
}
