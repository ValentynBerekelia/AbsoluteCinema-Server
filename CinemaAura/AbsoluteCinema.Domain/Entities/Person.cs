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
    public PersonRole PersonRole { get; private set; }
    
    private Person() { } 

    private Person(PersonId id, string name, string bio, DateTime birthDate, PersonRole personRole)
    {
        Id = id;
        Name = name;
        Bio = bio;
        BirthDate = birthDate;
        PersonRole = personRole;
    }

    public static Person Create(string name, string bio, DateTime birthDate, PersonRole personRole)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Person name is required.");
        }
        if (birthDate > DateTime.UtcNow)
        {
            throw new DomainException("Person birth date cannot be in the future.");
        }
        return new Person(PersonId.New(), name, bio, birthDate, personRole);
    }
    
    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Person name is required.");
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
            throw new DomainException("Person birth date cannot be in the future.");
        }
        BirthDate = birthDate;
    }

    public void ChangeMedia(Media media)
    {
        if (media.Type != MediaType.Image)
        {
            throw new DomainException("Media type must be an image.");
        }
        Media = media;
    }

    public void ChangePersonRole(PersonRole personRole)
    {
        PersonRole = personRole;
    }
    
}

public record struct PersonId(Guid Id)
{
    public static PersonId New() => new PersonId(Guid.NewGuid());
}