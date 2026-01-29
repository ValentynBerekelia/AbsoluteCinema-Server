using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.DTOs.Hall;
using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Infrastructure.EFQueries
{
    public class GetHallsDtoQuery(CinemaDbContext db) : IGetHallsDtoQuery
    {
        private readonly CinemaDbContext _db = db;
        public async Task<List<HallDto>> ExecuteAsync(GetHallsQuery query, CancellationToken ct)
        {
            return await _db.Halls
                .AsNoTracking()
                .Select(h => new HallDto(
                    h.Id,
                    h.HallName,
                    _db.Seats.Count(s => s.HallId == h.Id)
                ))
                .ToListAsync(ct);
        }
    }
}
