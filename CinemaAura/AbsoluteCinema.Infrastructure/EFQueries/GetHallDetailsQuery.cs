using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Infrastructure.EFQueries
{
    public class GetHallDetailsQuery(CinemaDbContext db) : IGetHallQueryHandler
    {
        private readonly CinemaDbContext _db = db;
        public async Task<GetHallQueryResponse?> ExecuteAsync(GetHallQuery query, CancellationToken ct)
        {
            return await _db.Halls
                .AsNoTracking()
                .Where(h => h.Id == query.HallId)
                .Select(h => new GetHallQueryResponse(
                    h.Id,
                    h.HallName,
                    _db.Seats
                        .Where(s => s.HallId == h.Id)
                        .Select(t => new SeatDto(
                            t.Id,
                            t.Row,
                            t.Number,
                            t.SeatType
                        ))
                        .ToList()
                ))
                .FirstOrDefaultAsync(ct);
        }
    }
}
