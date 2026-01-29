using AbsoluteCinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.DTOs.Hall
{
    public record HallDto(
        HallId Id,
        string Name,
        int NumberOfSeats
        );
}
