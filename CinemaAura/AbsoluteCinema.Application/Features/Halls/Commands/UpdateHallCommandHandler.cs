using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Halls.Commands;

public class UpdateHallCommandHandler(IHallRepository halls, IUnitOfWork unitOfWork) : IRequestHandler<UpdateHallCommand, Unit>
{
    private readonly IHallRepository _halls = halls;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateHallCommand request, CancellationToken cancellationToken)
    {
        var hallId = new HallId(request.HallId);
        var hall = await _halls.GetByIdForUpdateAsync(hallId, cancellationToken);

        if (hall is null)
            throw new KeyNotFoundException($"Hall with id {request.HallId} not found.");

        if (!string.IsNullOrWhiteSpace(request.HallName))
            hall.ChangeHallName(request.HallName);

        _halls.Update(hall);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public record UpdateHallCommand(
    Guid HallId,
    string? HallName
) : IRequest<Unit>;
