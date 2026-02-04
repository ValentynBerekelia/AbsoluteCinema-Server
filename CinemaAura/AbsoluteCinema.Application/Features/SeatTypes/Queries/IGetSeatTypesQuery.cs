using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.DTOs;

namespace AbsoluteCinema.Application.Features.SeatTypes.Queries
{
    public interface IGetSeatTypesQuery
    {
        Task<List<SeatTypeDto>> ExecuteTask(GetSeatTypesQuery query, CancellationToken ct);
    }
}
