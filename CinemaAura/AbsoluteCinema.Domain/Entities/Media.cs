using CinemaAura.Domain.Enums;
using CinemaAura.Domain.Exceptions;
using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Media : Entity<MediaId>
{
    public MediaType Type { get; private set; }
    public string Url { get; private set; }

    private Media() { }
    private Media(MediaId id, MediaType type, string url)
    {
        Id = id;
        Type = type;
        Url = url;
    }

    public static Media Create(MediaType type, string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new DomainException("Media URL is required.");
        }
        return new Media(MediaId.New(), type, url);
    }

    public void ChangeUrl(string url)
    {
        Url = url;
    }

    public void ChangeType(MediaType type)
    {
        Type = type;
    }

}

public record struct MediaId(Guid Id)
{
    public static MediaId New() => new MediaId(Guid.NewGuid());
}