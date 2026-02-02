using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.DTOs.Movie;
using MediatR;

namespace AbsoluteCinema.Application.Features.Genres.Queries
{
    public class GetGenresQueryHandler(IGetGenresQuery query)
    : IRequestHandler<GetGenresQuery, GetGenreResponse>
    {
        private readonly IGetGenresQuery _getGenres = query;
        public async Task<GetGenreResponse> Handle(GetGenresQuery request, CancellationToken ct)
        {
            var genre = await _getGenres.ExecuteAsync(request,ct);
            return new GetGenreResponse(genre);
        }
    }

    public record GetGenresQuery(Guid? MovieId) : IRequest<GetGenreResponse>;

    public record GetGenreResponse(
        List<GenreDto> Genres
        );
}
