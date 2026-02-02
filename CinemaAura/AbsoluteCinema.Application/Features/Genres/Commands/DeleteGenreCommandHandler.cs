using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Genres.Commands
{
    public class DeleteGenreCommandHandler(IGenreRepository genreRepository,IUnitOfWork unitOfWork)
        :IRequestHandler<DeleteGenreCommand,Unit>
    {
        private readonly IGenreRepository _genre = genreRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<Unit> Handle(DeleteGenreCommand command, CancellationToken ct)
        {
            var genreId = new GenreId(command.GenreId);
            var exist = await _genre.AnyAsync(genreId, ct);
            if (!exist)
            {
                throw new DomainException($"Genre with Id {command.GenreId} not found");
            }

            await _genre.DeleteAsync(genreId, ct);
            await _unit.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }

    public record DeleteGenreCommand(
        Guid GenreId
        ) : IRequest<Unit>;
}

