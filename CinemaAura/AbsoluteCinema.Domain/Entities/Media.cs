using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Media: Entity<MediaId>
{
    public string Type { get; private set; }
    public string Url { get; private set; }

    private Media() { }
    private Media(MediaId id, string type, string url)
    {
        Id = id;
        Type = type;
        Url = url;
    }

    public static Media Create(string type, string url)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Media type cannot be null or empty.", nameof(type));
        }
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Media URL cannot be null or empty.", nameof(url));
        }
        return new Media(MediaId.New(), type, url);
    }
    
    public void ChangeUrl(string url)
    {
        Url = url;
    }
    
    public void ChangeType(string type)
    {
        Type = type;
    }
    
}

public record struct MediaId(Guid Id)
{
    public static MediaId New() => new MediaId(Guid.NewGuid());
}