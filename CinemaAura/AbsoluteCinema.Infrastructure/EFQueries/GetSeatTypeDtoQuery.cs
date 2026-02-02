using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.SeatTypes.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries
{
    public class GetSeatTypeDtoQuery(CinemaDbContext dbContext) : IGetSeatTypesQuery
    {
        private readonly CinemaDbContext _db = dbContext;
        public async Task<List<SeatTypeDto>> ExecuteTask(GetSeatTypesQuery query, CancellationToken ct)
        {
            var result =  _db.SeatTypes
                .AsNoTracking()
                .Select(s => new SeatTypeDto(s.Id,s.TypeName));

            return await result.ToListAsync(ct);
        }
    }
}
