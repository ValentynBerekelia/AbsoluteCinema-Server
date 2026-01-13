using CinemaAura.Domain.Primitives;

namespace IdentityService.Domain.Entities;

public class Movie:  Entity<MovieId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public float Rate{get; private set;}
    public int Age_limit{get; private set;}
    
    private readonly HashSet<GenreId> _genreIds = new HashSet<GenreId>();
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;
    
    private readonly HashSet<MediaId> _mediaIds = new HashSet<MediaId>();
    public IReadOnlyCollection<MediaId> MediaIds => _mediaIds;
    
    private readonly HashSet<ActorId> _actorIds = new HashSet<ActorId>();
    public IReadOnlyCollection<ActorId> ActorIds => _actorIds;

    private Movie(MovieId movieId, string name,  string description, float rate, int age_limit)
    {
        Id = movieId;
        Name = name;
        Description = description;
        Rate = rate;
        Age_limit = age_limit;
        
    }

    public static Movie Create(string name,  string description, float rate, int age_limit)
    {
        return new Movie(MovieId.New(), name, description, rate, age_limit);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }

    public void ChangeRate(float rate)
    {
        Rate = rate;
    }

    public void ChangeAge(int age_limit)
    {
        Age_limit = age_limit;
    }


}

public record MovieId(Guid Id)
{
    public static MovieId New() => new MovieId(Guid.NewGuid());
}