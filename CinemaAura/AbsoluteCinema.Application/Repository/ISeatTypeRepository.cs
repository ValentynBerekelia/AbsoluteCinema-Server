using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository
{
    public interface ISeatTypeRepository : IRepository<SeatTypeId,SeatType>
    {
    }
}
