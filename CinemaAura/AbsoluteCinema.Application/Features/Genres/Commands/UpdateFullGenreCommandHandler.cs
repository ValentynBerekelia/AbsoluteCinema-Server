using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace AbsoluteCinema.Application.Features.Genres.Commands
{
    public class UpdateFullGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateFullGenreCommand, Unit>
    {
        private readonly IGenreRepository _genre = genreRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<Unit> Handle(UpdateFullGenreCommand request, CancellationToken ct)
        {
            var genreId = new GenreId(request.GenreId);
            var genre = await _genre.GetByIdForUpdateAsync(genreId, ct);
            if (genre is null)
            {
                throw new KeyNotFoundException($"Genre with id {request.GenreId} not found");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                genre.ChangeName(request.Name);
            }

            _genre.Update(genre);
            await _unit.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }

    public record UpdateFullGenreCommand(
        Guid GenreId,
        string Name
        ) :IRequest<Unit>;
}
