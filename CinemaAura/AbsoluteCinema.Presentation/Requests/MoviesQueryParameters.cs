using System.ComponentModel.DataAnnotations;
using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests;

public class MoviesQueryParameters
{
    public string? SearchTerm { get; set; }
    
    public string[]? Genres { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public string SortColumn { get; set; } = "name";

    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}