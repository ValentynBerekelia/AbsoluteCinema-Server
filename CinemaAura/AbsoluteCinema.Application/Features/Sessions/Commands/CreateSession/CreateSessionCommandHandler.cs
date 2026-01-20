using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Interfaces;
using MediatR;

namespace CinemaAura.Application.Features.Sessions.Commands.CreateSession;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Guid>
{
    private readonly ISessionRepository _repository;

    public CreateSessionCommandHandler(ISessionRepository repository)
    { 
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var movieId = new MovieId(request.MovieId);
        var hallId = new HallId(request.HallId);

        // (Валідація дати відбудеться всередині методу Session.Create)
        var session = Session.Create(
            movieId,
            hallId,
            request.StartTime
        );

        await _repository.AddSessionAsync(session, cancellationToken);

        foreach (var priceDto in request.Prices) 
        {
            var seatTypeId = new SeatTypeId(priceDto.SeatTypeId);

            var typePrice = TypePrice.Create(
                session.Id,
                seatTypeId,
                priceDto.Price
            );

            await _repository.AddTypePriceAsync(typePrice, cancellationToken);
        }
        await _repository.SaveChangesAsync(cancellationToken);

        return session.Id.Id;
    }

}
