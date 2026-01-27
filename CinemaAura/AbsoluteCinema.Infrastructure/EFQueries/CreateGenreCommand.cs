using AbsoluteCinema.Application.Features.Movies.Command;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class CreateGenreCommand(CinemaDbContext db) : ICreateGenreCommend
{
    private readonly CinemaDbContext _db = db;

    public async Task<List<Genre>> ExecuteAsync(List<string> genreNames, CancellationToken ct)
    {
        var normalizedNames = genreNames
            .Select(g => g.Trim())
            .Distinct()
            .ToList();

        var existingGenres = await _db.Genres
            .Where(g => normalizedNames.Contains(g.Name))
            .ToListAsync(ct);

        var existingNames = existingGenres
            .Select(g => g.Name)
            .ToHashSet();

        var result = new List<Genre>(existingGenres);

        foreach (var name in normalizedNames)
        {
            if (!existingNames.Contains(name))
            {
                var genre = Genre.Create(name);
                _db.Genres.Add(genre);
                result.Add(genre);
            }
        }

        await _db.SaveChangesAsync(ct);

        return result;
    }
}