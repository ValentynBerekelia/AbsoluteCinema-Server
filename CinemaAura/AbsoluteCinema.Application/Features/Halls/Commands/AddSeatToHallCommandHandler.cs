using AbsoluteCinema.Application.Features.Halls.Commands;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Halls.Commands;

public class AddSeatToHallCommandHandler : IRequestHandler<AddSeatToHallCommand, AddSeatToHallResponse>
{
    private readonly IHallRepository _halls;
    private readonly ISeatRepository _seats;
    private readonly IUnitOfWork _unitOfWork;

    public AddSeatToHallCommandHandler(IHallRepository halls, ISeatRepository seats, IUnitOfWork unitOfWork)
    {
        _halls = halls;
        _seats = seats;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddSeatToHallResponse> Handle(AddSeatToHallCommand request, CancellationToken ct)
    {
        var hallId = new HallId(request.HallId);
        var hall = await _halls.GetByIdForUpdateAsync(hallId, ct);
        if (hall is null)
            throw new ArgumentException($"Hall with id {request.HallId} not found.");

        var seatTypeId = request.SeatTypeId;

        var created = new List<SeatOutputModel>();

        foreach (var s in request.Seats)
        {
            var seat = Seat.Create(hallId, (short)s.Row, (short)s.Number, seatTypeId);
            await _seats.AddAsync(seat, ct);
            hall.AddSeat(seat.Id);

            created.Add(new SeatOutputModel(
                seat.Id.Id,
                seat.Row,
                seat.Number,
                seat.SeatTypeId,
                string.Empty));
        }

        _halls.Update(hall);

        await _unitOfWork.SaveChangesAsync(ct);

        return new AddSeatToHallResponse(hall.Id.Id, created);
    }
}

public record AddSeatToHallCommand(
    Guid HallId,
    SeatTypeId SeatTypeId,
    IReadOnlyCollection<SeatInputModel> Seats
    ) : IRequest<AddSeatToHallResponse>;

public record SeatInputModel(
    int Row,
    int Number
    );

public record AddSeatToHallResponse(
    Guid HallId,
    IReadOnlyCollection<SeatOutputModel> Seats
);

public record SeatOutputModel(
    Guid SeatId,
    int Row,
    int Number,
    SeatTypeId SeatTypeId,
    string SeatTypeName);


