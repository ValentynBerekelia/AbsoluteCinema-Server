using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.DTOs.Movie
{
    public record MovieUpdatePartialRequest(
         string? Name,
         string? Description,
         decimal? Rate,
         int? AgeLimit,
         int? DurationSeconds,
         string? Country,
         string? Studio,
         string? Language
    );
}
