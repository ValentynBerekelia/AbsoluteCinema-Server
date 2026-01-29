using AbsoluteCinema.Application.DTOs.Hall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Halls.Queries
{
    public interface IGetHallsDtoQuery
    {
        Task<List<HallDto>> ExecuteAsync(GetHallsQuery query, CancellationToken ct);
    }
}
