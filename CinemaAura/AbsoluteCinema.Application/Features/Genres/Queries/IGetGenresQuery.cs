using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.DTOs.Movie;

namespace AbsoluteCinema.Application.Features.Genres.Queries
{
    public interface IGetGenresQuery
    {
        Task<List<GenreDto>> ExecuteAsync(GetGenresQuery query, CancellationToken ct);
    }
}
