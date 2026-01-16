using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Movie : AggregateRoot<MovieId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Rate { get; private set; }
    public int AgeLimit { get; private set; }
    // mb add release date, duration, director, etc.

    private readonly HashSet<MovieGenre> _genreIds = new HashSet<MovieGenre>();
    public IReadOnlyCollection<MovieGenre> GenreIds => _genreIds;

    private readonly HashSet<MovieMedia> _mediaIds = new HashSet<MovieMedia>();
    public IReadOnlyCollection<MovieMedia> MediaIds => _mediaIds;

    private readonly HashSet<MovieActor> _actorIds = new HashSet<MovieActor>();
    public IReadOnlyCollection<MovieActor> ActorIds => _actorIds;

    private Movie() { }
    private Movie(MovieId movieId, string name, string description, decimal rate, int age_limit)
    {
        Id = movieId;
        Name = name;
        Description = description;
        Rate = rate;
        AgeLimit = age_limit;

    }

    public static Movie Create(string name, string description, decimal rate, int age_limit)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Movie name cannot be null or empty.", nameof(name));
        }
        if (rate < 0 || rate > 10)
        {
            throw new ArgumentException("Rate must be between 0 and 10.", nameof(rate));
        }
        if (age_limit < 0)
        {
            throw new ArgumentException("Age limit cannot be negative.", nameof(age_limit));
        }
        return new Movie(MovieId.New(), name, description, rate, age_limit);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Movie name cannot be null or empty.", nameof(name));
        }
        Name = name;
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }

    public void ChangeRate(decimal rate)
    {
        if (rate < 0 || rate > 10)
        {
            throw new ArgumentException("Rate must be between 0 and 10.", nameof(rate));
        }
        Rate = rate;
    }

    public void ChangeAge(int age_limit)
    {
        if (age_limit < 0)
        {
            throw new ArgumentException("Age limit cannot be negative.", nameof(age_limit));
        }
        AgeLimit = age_limit;
    }

    public void AddGenre(GenreId genreId)
    {
        _genreIds.Add(new MovieGenre(genreId));
    }

    public void RemoveGenre(GenreId genreId)
    {
        _genreIds.Remove(new MovieGenre(genreId));
    }

    public void ClearGenres()
    {
        _genreIds.Clear();
    }

    public void AddMedia(MediaId mediaId)
    {
        _mediaIds.Add(new MovieMedia(mediaId));
    }

    public void RemoveMedia(MediaId mediaId)
    {
        _mediaIds.Remove(new MovieMedia(mediaId));
    }

    public void ClearMedias()
    {
        _mediaIds.Clear();
    }

    public void AddActor(ActorId actorId)
    {
        _actorIds.Add(new MovieActor(actorId));
    }

    public void RemoveActor(ActorId actorId)
    {
        _actorIds.Remove(new MovieActor(actorId));
    }

    public void ClearActors()
    {
        _actorIds.Clear();
    }

}

public record struct MovieId(Guid Id)
{
    public static MovieId New() => new MovieId(Guid.NewGuid());
}

public record MovieGenre(GenreId GenreId);

public record MovieMedia(MediaId MediaId);

public record MovieActor(ActorId ActorId);
