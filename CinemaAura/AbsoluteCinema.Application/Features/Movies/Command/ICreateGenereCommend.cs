using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Features.Movies.Command;

public interface ICreateGenreCommend
{
    Task<List<Genre>> ExecuteAsync(List<string> genres, CancellationToken ct);
}