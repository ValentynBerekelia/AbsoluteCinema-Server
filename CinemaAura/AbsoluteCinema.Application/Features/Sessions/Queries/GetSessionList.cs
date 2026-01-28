using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using MediatR;
using System.Linq;

namespace AbsoluteCinema.Application.Features.Sessions.Queries;

public record SessionListItemDto(
    Guid Id,
    Guid MovieId,
    Guid HallId,//fix
    MovieFormat Format,
    DateTime StartDateTime
);

public record PagedSessionResponse(
    List<SessionListItemDto> Sessions,
    int TotalCount,
    int PageNumber,
    int PageSize
);

public record GetSessionsListQuery : IRequest<PagedSessionResponse>
{
    public Guid MovieId { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SortColumn { get; init; }
    public SortOrder SortOrder { get; init; } = SortOrder.Asc;
}

public class GetSessionsListHandler : IRequestHandler<GetSessionsListQuery, PagedSessionResponse>
{
    private readonly ISessionRepository _repository;

    public GetSessionsListHandler(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedSessionResponse> Handle(GetSessionsListQuery request, CancellationToken cancellationToken)
    {
        var (sessions, totalCount) = await _repository.GetPagedSessionsAsync(
            request.MovieId,
            request.PageNumber,
            request.PageSize,
            request.SortColumn,
            request.SortOrder,
            cancellationToken
        );

        var sessionDtos = sessions.Select(s => new SessionListItemDto(
            s.Id.Id,
            s.MovieId.Id,
            s.HallId.Id,
            s.Format,
            s.StartDateTime
        )).ToList();

        return new PagedSessionResponse(sessionDtos, totalCount, request.PageNumber, request.PageSize);
    }
}