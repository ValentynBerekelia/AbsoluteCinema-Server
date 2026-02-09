namespace AbsoluteCinema.Domain.Enums;

public static class MediaTypeExtensions
{
    public static string GetFolderName(this MediaType type) => type switch
    {
        MediaType.PosterImage => "posters",
        MediaType.Image => "stills",
        MediaType.Video => "trailers",
        MediaType.PersonImage => "persons",
        MediaType.BannerImage => "banners",
        _ => "other"
    };
}