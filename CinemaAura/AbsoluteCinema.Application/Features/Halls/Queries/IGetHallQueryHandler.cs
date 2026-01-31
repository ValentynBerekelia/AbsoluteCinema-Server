using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Halls.Queries
{
    public interface IGetHallQueryHandler
    {
        Task<GetHallQueryResponse?> ExecuteAsync(GetHallQuery request, CancellationToken ct);
    }
}
