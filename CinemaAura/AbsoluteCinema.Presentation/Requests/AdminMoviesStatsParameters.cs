namespace AbsoluteCinema.Requests;

public class AdminMoviesStatsParameters
{
    public string? SearchTerm { get; set; }
    public int PageSize { get; set; } = 10;
    public Guid? LastMovieId { get; set; }
}