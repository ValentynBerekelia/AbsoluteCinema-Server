using CinemaAura.Domain.Primitives;

namespace IdentityService.Domain.Entities;

public class Genre:  Entity<GenreId>
{
    public string Name { get; private set; }

    private Genre(GenreId genreId, string name)
    {
        Id = genreId;
        Name = name;
    }

    public static Genre Create(string name)
    {
        return new Genre(GenreId.New(), name);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
    
    
}

public record GenreId(Guid Id)
{
    public static GenreId New() => new GenreId(Guid.NewGuid());
}