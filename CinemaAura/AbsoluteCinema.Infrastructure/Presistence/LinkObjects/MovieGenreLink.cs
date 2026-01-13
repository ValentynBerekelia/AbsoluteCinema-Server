namespace CinemaAura.Infrastructure.Presistence.Configurations.LinkObjects;

public class MovieGenreLink
{
    public Guid MovieId { get; private set; }
    public Guid GenreId { get; private set; }
    private MovieGenreLink() { }
    public MovieGenreLink(Guid movieId, Guid genreId)
    { 
        MovieId = movieId;
        GenreId = genreId;
    }
    
}