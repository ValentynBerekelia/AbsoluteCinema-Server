using AbsoluteCinema.Application.DTOs.Hall;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbsoluteCinema.Application.Features.Halls.Queries.GetHallsQuery;

namespace AbsoluteCinema.Application.Features.Halls.Queries
{
    public class GetHallsQueryHandler(IGetHallsDtoQuery getHallsDto) : IRequestHandler<GetHallsQuery, GetHallsResponse>
    {
        private readonly IGetHallsDtoQuery _getHallsDto = getHallsDto;
        public async Task<GetHallsResponse> Handle(GetHallsQuery request, CancellationToken ct)
        {
            var hallDto = await _getHallsDto.ExecuteAsync(request, ct);
            return new GetHallsResponse(hallDto);

        }
    }
    public record GetHallsQuery : IRequest<GetHallsResponse>;
    public record GetHallsResponse(
        List<HallDto> Halls
        ){ }
}
