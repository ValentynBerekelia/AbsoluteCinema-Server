using System.Linq.Expressions;
using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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

        newQuery = newQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);

        return await newQuery.Select(m => new MovieDto(
            m.Id,
            m.Medias
                .Where(c => c.Type == MediaType.BannerImage)
                .Select(j => j.Url)
                .FirstOrDefault(),
            m.Rate,
            m.AgeLimit,
            m.Duration,
            m.Genres.Select(g => g.Name)
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