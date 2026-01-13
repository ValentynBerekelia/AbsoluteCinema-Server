namespace CinemaAura.Infrastructure.Presistence.Configurations.LinkObjects;

public class MovieMediaLink
{
    public Guid MovieId { get; private set; }
    public Guid MediaId { get; private set; }
    private MovieMediaLink() { }
    public MovieMediaLink(Guid movieId, Guid mediaId)
    { 
        MovieId = movieId;
        MediaId = mediaId;  
    }
    
}