namespace CinemaAura.Infrastructure.Presistence.Configurations.LinkObjects;

public class MovieActorLink
{
    public Guid MovieId { get; private set; }
    public Guid ActorId { get; private set; }
    private MovieActorLink() { }
    public MovieActorLink(Guid movieId, Guid actorId)
    { 
        MovieId = movieId;
        ActorId = actorId;
    }
    
}