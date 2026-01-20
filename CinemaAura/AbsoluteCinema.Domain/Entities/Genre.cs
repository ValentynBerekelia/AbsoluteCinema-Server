using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Genre:  Entity<GenreId>
{
    public string Name { get; private set; }
    private Genre() { }

    private Genre(GenreId genreId, string name)
    {
        Id = genreId;
        Name = name;
    }

    public static Genre Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Genre name cannot be null or empty.", nameof(name));
        }
        return new Genre(GenreId.New(), name);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Genre name cannot be null or empty.", nameof(name));
        }
        Name = name;
    }
    
    
}

public record struct GenreId(Guid Id)
{
    public static GenreId New() => new GenreId(Guid.NewGuid());
}