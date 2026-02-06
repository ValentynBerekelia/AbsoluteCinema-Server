using System.Linq.Expressions;
using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetMoviesDtoQuery(CinemaDbContext db) : IGetMoviesDtoQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<List<MovieDto>> ExecuteAsync(GetMoviesQuery query, CancellationToken ct)
    {
        var newQuery = _db.Movies.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            newQuery = newQuery
                .Where(m => EF.Functions.Like(m.Name, $"%{query.SearchTerm}%"));
        }
        
        if (query.Genres?.Any() == true)
        {
            newQuery = newQuery.Where(m =>
                m.Genres.Any(g => query.Genres.Contains(g.Name))
            );
        }

        if (!string.IsNullOrWhiteSpace(query.SortColumn))
        {
            var expr = GetSortExpression(query.SortColumn);
            newQuery = query.SortOrder == SortOrder.Desc ?
                newQuery.OrderByDescending(expr) : newQuery.OrderBy(expr);
        }

        var first = query.FirstDate is not null
            ? DateTime.SpecifyKind(
                DateOnly.Parse(query.FirstDate).ToDateTime(TimeOnly.MinValue),
                DateTimeKind.Utc)
            : DateTime.UtcNow.Date;

        var second = query.SecondDate is not null
            ? DateTime.SpecifyKind(
                DateOnly.Parse(query.SecondDate).ToDateTime(TimeOnly.MinValue),
                DateTimeKind.Utc).AddDays(1)
            : first.AddDays(1);
        
        newQuery = newQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);
        
        return await newQuery.Select(m => new MovieDto(
            m.Id.Id,
            m.Name,
            m.Medias
                .Where(c => c.Type == MediaType.PosterImage)
                .Select(j => j.Url)
                .FirstOrDefault(),
            m.Rate,
            m.AgeLimit,
            m.Duration,
            m.Genres.Select(g => new GenreDto(g.Id.Id, g.Name) ).ToList(),
            _db.Sessions
                .Where(s => s.MovieId == m.Id && s.StartDateTime >= first && s.StartDateTime < second)
                .Select(s => new SessionDto (
                    s.Id.Id,
                    s.StartDateTime,
                    s.Format
                ))
                .ToList()
        )).ToListAsync(ct);
    }

    private Expression<Func<Movie, object>> GetSortExpression(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "name" => m => m.Name,
            "rate" => m=> m.Rate,
            "ageLimit" => m => m.AgeLimit,
            _ => m => m.Id
        };
    }
}