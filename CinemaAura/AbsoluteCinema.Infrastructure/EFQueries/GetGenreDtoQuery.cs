using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Features.Genres.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Infrastructure.EFQueries
{
    public class GetGenreDtoQuery(CinemaDbContext db) : IGetGenresQuery
    {
        private readonly CinemaDbContext _db = db;
        public async Task<List<GenreDto>> ExecuteAsync(GetGenresQuery query, CancellationToken ct)
        {
            if (query.MovieId is null)
            {
                return await _db.Genres
                    .AsNoTracking()
                    .OrderBy(g => g.Name)
                    .Select(g => new GenreDto(
                        g.Id.Id,     // якщо GenreId має поле Id
                        g.Name
                    ))
                    .ToListAsync(ct);
            }
            var movieId = new MovieId(query.MovieId.Value);

            return await _db.Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .SelectMany(m => m.Genres)
                .Distinct()
                .OrderBy(g => g.Name)
                .Select(g => new GenreDto(
                    g.Id.Id,
                    g.Name
                ))
                .ToListAsync(ct);
        }
    }
}
