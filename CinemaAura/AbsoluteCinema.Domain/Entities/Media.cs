using CinemaAura.Domain.Primitives;

namespace IdentityService.Domain.Entities;

public class Media: Entity<MediaId>
{
    public string Type { get; private set; }
    public string Url { get; private set; }

    private Media(MediaId id, string type, string url)
    {
        Id = id;
        Type = type;
        Url = url;
    }

    public static Media Create(string type, string url)
    {
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

public record MediaId(Guid Id)
{
    public static MediaId New() => new MediaId(Guid.NewGuid());
}