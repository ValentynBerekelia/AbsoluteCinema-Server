using System.ComponentModel.DataAnnotations;
using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests;

public class MoviesQueryParameters
{
    public string? SearchTerm { get; set; }
    
    public string[]? Genres { get; set; }
    
    /// <example>2026-01-31</example>
    public string? FirstDate { get; init; }
    
    /// <example>2026-12-31</example>
    public string? SecondDate { get; init; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public string SortColumn { get; set; } = "name";

    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}