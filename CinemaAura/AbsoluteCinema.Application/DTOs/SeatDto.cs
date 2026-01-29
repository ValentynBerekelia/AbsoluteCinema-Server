using AbsoluteCinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.DTOs
{
    public record SeatDto(
        SeatId SeatId,
        short Row,
        short Number,
        SeatType SeatType
        );
    
}
