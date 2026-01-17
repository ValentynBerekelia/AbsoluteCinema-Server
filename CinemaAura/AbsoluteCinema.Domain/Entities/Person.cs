using CinemaAura.Domain.Enums;
using CinemaAura.Domain.Exceptions;
using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Person: Entity<PersonId>
{
    public string Name { get; private set; }
    public string Bio{ get; private set; }
    public DateTime BirthDate { get; private set; }
    public MediaId? MediaId { get; private set; }  
    public Media? Media { get; private set; }  
    
    private Person() { } 

    private Person(PersonId id, string name, string bio, DateTime birthDate)
    {
        Id = id;
        Name = name;
        Bio = bio;
        BirthDate = birthDate;
    }

    public static Person Create(string name, string bio, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Actor name cannot be null or empty.", nameof(name));
        }
        if (birthDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));
        }
        return new Person(PersonId.New(), name, bio, birthDate);
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

    public void ChangeMedia(Media media)
    {
        if (media.Type != MediaType.Image)
        {
            throw new DomainException("Incorrect media type for person. It supposed to be image.");
        }
        Media = media;
    }
    
}

public record struct PersonId(Guid Id)
{
    public static PersonId New() => new PersonId(Guid.NewGuid());
}