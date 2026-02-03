using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories
{
    public class SeatTypeRepository : BaseRepository<SeatTypeId,SeatType,CinemaDbContext>,ISeatTypeRepository
    {
        public SeatTypeRepository(CinemaDbContext db) : base(db)
        {

        }

    }
}
