using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Movie : AggregateRoot<MovieId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Rate { get; private set; }
    public int AgeLimit { get; private set; }
    public TimeSpan Duration { get; private set; }
    
    //In my opinion, creating new entities for the fields below is too much.
    public string Country { get; private set; }
    public string Studio { get; private set; }
    public string Language { get; private set; }

    private readonly List<Genre> _genres = new List<Genre>();
    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();

    private readonly List<Media> _medias = new List<Media>();
    public IReadOnlyCollection<Media> Medias => _medias.AsReadOnly();

    private readonly List<Person> _persons = new List<Person>();
    public IReadOnlyCollection<Person> Persons => _persons.AsReadOnly();

    private Movie() { }
    private Movie(MovieId movieId, string name, string description, decimal rate, int ageLimit, TimeSpan duration, string country, string studio, string language)
    {
        Id = movieId;
        Name = name;
        Description = description;
        Rate = rate;
        AgeLimit = ageLimit;
        Duration = duration;
        Language = language;
        Studio = studio;
        Country = country;
    }

    public static Movie Create(string name, string description, decimal rate, int ageLimit, TimeSpan duration, string country, string studio, string language)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Movie name is required.");
        }
        if (string.IsNullOrWhiteSpace(language))
        {
            throw new DomainException("Movie language is required.");
        }
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new DomainException("Movie country is required.");
        }
        if (string.IsNullOrWhiteSpace(studio))
        {
            throw new DomainException("Movie studio is required.");
        }
        if (rate < 0 || rate > 10)
        {
            throw new DomainException("Movie rating must be between 0 and 10.");
        }
        if (duration <= TimeSpan.Zero)
        {
            throw new DomainException("Movie duration must be greater than zero.");
        }
        if (ageLimit < 0)
        {
            throw new DomainException("Movie age limit cannot be negative.");
        }
        return new Movie(MovieId.New(), name, description, rate, ageLimit, duration, country, studio, language);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Movie name is required.");
        }
        Name = name;
    }
    
    public void ChangeCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new DomainException("Movie country is required.");
        }
        Country = country;
    }
    public void ChangeLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            throw new DomainException("Movie language is required.");
        }
        Language = language;
    }
    public void ChangeStudio(string studio)
    {
        if (string.IsNullOrWhiteSpace(studio))
        {
            throw new DomainException("Movie studio is required.");
        }
        Studio = studio;
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }

    public void ChangeRate(decimal rate)
    {
        if (rate < 0 || rate > 10)
        {
            throw new DomainException("Movie rating must be between 0 and 10.");
        }
        Rate = rate;
    }

    public void ChangeAge(int age_limit)
    {
        if (age_limit < 0)
        {
            throw new DomainException("Movie age limit cannot be negative.");
        }
        AgeLimit = age_limit;
    }

    public void ChangeDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new DomainException("Duration must be greater than zero.");
        }

        Duration = duration;
    }
    public void AddGenre(Genre genre)
    {
        if (_genres.Any(g => g.Id != genre.Id))
            _genres.Add(genre);
    }

    public void RemoveGenre(GenreId genreId)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == genreId);
        if (genre != null) _genres.Remove(genre);
    }

    public void ClearGenres()
    {
        _genres.Clear();
    }

    public void AddMedia(Media media)
    {
        if(_medias.Any(m => m.Id != media.Id))
            _medias.Add(media);
    }

    public void RemoveMedia(MediaId mediaId)
    {
        var media = _medias.FirstOrDefault(m => m.Id == mediaId);
        if (media != null)_medias.Remove(media);
    }

    public void ClearMedias()
    {
        _medias.Clear();
    }

    public void AddPerson(Person person)
    {
        var personExists = _persons.Any(p => p.Id == person.Id);
        if (!personExists)
            _persons.Add(person);
    }

    public void RemovePerson(Person person)
    {
        var personExists = _persons.Any(p => p.Id == person.Id);
        if (personExists)
            _persons.Remove(person);
    }

    public void ClearPersons()
    {
        _persons.Clear();
    }

}

public record struct MovieId(Guid Id)
{
    public static MovieId New() => new MovieId(Guid.NewGuid());
}

public record MovieGenre(GenreId GenreId);

public record MovieMedia(MediaId MediaId);

public record MovieActor(ActorId ActorId);
