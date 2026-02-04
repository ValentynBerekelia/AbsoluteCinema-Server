using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.SeatTypes.Queries
{
    public class GetSeatTypesQueryHandler(IGetSeatTypesQuery query, IUnitOfWork unitOfWork)
    : IRequestHandler<GetSeatTypesQuery, GetSeatTypesResponse>
    {
        private readonly IGetSeatTypesQuery _getSeatTypes = query;
        public async Task<GetSeatTypesResponse> Handle(GetSeatTypesQuery request, CancellationToken ct)
        {
            var seatTypesDtos = await _getSeatTypes.ExecuteTask(request, ct);
            return new GetSeatTypesResponse(seatTypesDtos);
        }
    }

    public record GetSeatTypesQuery():IRequest<GetSeatTypesResponse>;

    public record GetSeatTypesResponse(List<SeatTypeDto> SeatTypes);
}
