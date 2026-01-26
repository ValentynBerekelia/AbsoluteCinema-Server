using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.DTOs.Movie
{
    public sealed record MovieUpdatePartialRequest
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? Rate { get; init; }
        public int? AgeLimit { get; init; }
        public int? DurationSeconds { get; init; }
        public string? Country { get; init; }
        public string? Studio { get; init; }
        public string? Language { get; init; }
    }
}
