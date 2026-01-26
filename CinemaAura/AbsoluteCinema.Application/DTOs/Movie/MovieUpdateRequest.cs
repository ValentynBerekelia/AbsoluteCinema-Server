using AbsoluteCinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.DTOs.Movie
{
    public record MovieUpdateRequest(
        string Name,
        string Description, 
        decimal Rate, 
        int AgeLimit, 
        int DurationSecond, 
        string Country, 
        string Studio,
        string Language);
}
