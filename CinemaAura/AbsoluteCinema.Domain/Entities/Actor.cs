using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Actor: Entity<ActorId>
{
    public string Name { get; private set; }
    public string Bio{ get; private set; }
    public DateTime BirthDate { get; private set; }
    private Actor() { } 

    private Actor(ActorId id, string name, string bio, DateTime birthDate)
    {
        Id = id;
        Name = name;
        Bio = bio;
        BirthDate = birthDate;
    }

    public static Actor Create(string name, string bio, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Actor name cannot be null or empty.", nameof(name));
        }
        if (birthDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));
        }
        return new Actor(ActorId.New(), name, bio, birthDate);
    }
    
    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Actor name cannot be null or empty.", nameof(name));
        }
        Name = name;
    }

    public void ChangeBio(string bio)
    {
        Bio = bio;
    }
    public void ChangeBirthDate(DateTime birthDate)
    {
        if (birthDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));
        }
        BirthDate = birthDate;
    }
    
}

public record struct ActorId(Guid Id)
{
    public static ActorId New() => new ActorId(Guid.NewGuid());
}