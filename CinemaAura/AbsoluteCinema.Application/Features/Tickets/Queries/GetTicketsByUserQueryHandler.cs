using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Application.EFQueries;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using static AbsoluteCinema.Application.Features.Tickets.Queries.GetTicketsByUserQueryHandler;

namespace AbsoluteCinema.Application.Features.Tickets.Queries;

public sealed class GetTicketsByUserQueryHandler(IGetTicketsByUserDtoQuery query)
    : IRequestHandler<GetTicketsByUserQuery, List<GetTicketForUserResponse>>
{
    private readonly IGetTicketsByUserDtoQuery _query = query;

    public async Task<List<GetTicketForUserResponse>> Handle(GetTicketsByUserQuery request, CancellationToken ct)
        => await _query.ExecuteAsync(request, ct) ?? new();
public sealed record GetTicketsByUserQuery(UserId UserId)
        : IRequest<List<GetTicketForUserResponse>>;

}