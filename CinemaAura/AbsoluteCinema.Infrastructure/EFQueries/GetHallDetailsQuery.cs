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
            // 1. Отримуємо основні дані залу та місць одним запитом
            var hallData = await _db.Halls
                .AsNoTracking()
                .Where(h => h.Id == query.HallId)
                .Select(h => new
                {
                    h.Id,
                    h.HallName,
                    Seats = _db.Seats
                        .Where(s => s.HallId == h.Id)
                        .Select(t => new SeatDto(
                            t.Id,
                            t.Row,
                            t.Number,
                            t.SeatTypeId
                        )).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (hallData == null) return null;

            var usedTypeIds = hallData.Seats
                .Select(s => s.SeatTypeId)
                .Distinct()
                .ToList();

            var availableTypes = await _db.SeatTypes
                .AsNoTracking()
                .Where(st => usedTypeIds.Contains(st.Id))
                .Select(st => new SeatTypeDto(st.Id, st.TypeName))
                .ToListAsync(ct);

            return new GetHallQueryResponse(
                hallData.Id,
                hallData.HallName,
                hallData.Seats,
                availableTypes
            );
        }
    }
}
