using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Halls.Queries
{
    public class GetHallQueryHandler(IGetHallQueryHandler detaildQuery)
        :IRequestHandler<GetHallQuery, GetHallQueryResponse>
    {
        public async Task<GetHallQueryResponse> Handle(GetHallQuery request, CancellationToken ct)
        {
            var hall = await detaildQuery.ExecuteAsync(request, ct);
            if(hall is null)
            {
                throw new Exception($"Hall {request.HallId} Not Found");
            }
            return hall;
        }
    }
    public record GetHallQuery : IRequest<GetHallQueryResponse>
    {
        public HallId HallId { get; }
        public GetHallQuery(HallId id) => HallId = id;
    }
    public record GetHallQueryResponse(
        HallId HallId,
        string Name,
        IEnumerable<SeatDto> Seats
        ) { }
}
