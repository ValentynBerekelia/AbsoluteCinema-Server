using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Genres.Commands
{
    public class CreateGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, CreateGenreResponse>
    {
        private readonly IGenreRepository _genre = genreRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<CreateGenreResponse> Handle(CreateGenreCommand request, CancellationToken ct)
        {
            var genre = Genre.Create(request.GenreName);

            await _genre.AddAsync(genre, ct);
            await _unit.SaveChangesAsync(ct);

            return new CreateGenreResponse(genre.Id.Id, genre.Name);
        }
    }

    public record CreateGenreRequest(
        string GenreName
        );   
        
    public record CreateGenreCommand(
        string GenreName
        ) :IRequest<CreateGenreResponse>;

    public record CreateGenreResponse(
        Guid GenreId,
        string GenreName
        );
}
