using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    private readonly ISessionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSessionCommandHandler(ISessionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateSessionResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var movieId = new MovieId(request.MovieId);
        var hallId = new HallId(request.HallId);
        var format = request.Format;

        // (Валідація дати відбудеться всередині методу Session.Create)
        var session = Session.Create(
            movieId,
            hallId,
            request.StartTime,
            format
        );

        await _repository.AddAsync(session, cancellationToken);

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

        return new CreateSessionResponse(session.Id.Id);
    }
}
