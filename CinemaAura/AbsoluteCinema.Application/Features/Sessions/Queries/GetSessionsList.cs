using AbsoluteCinema.Application.Repository;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Queries;
public record SessionListItemDto(
    Guid Id,
    Guid MovieId,
    Guid HallId,
    DateTime StartTime
);

public record GetSessionsListQuery() : IRequest<List<SessionListItemDto>>;

public class GetSessionsListHandler : IRequestHandler<GetSessionsListQuery, List<SessionListItemDto>>
{
    private readonly ISessionRepository _repository;

    public GetSessionsListHandler(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SessionListItemDto>> Handle(GetSessionsListQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _repository.GetAllWithDetailsAsync(cancellationToken);

        return sessions.Select(s => new SessionListItemDto(
            s.Id.Id,
            s.MovieId.Id,
            s.HallId.Id,
            s.StartDateTime
        )).ToList();
    }
}