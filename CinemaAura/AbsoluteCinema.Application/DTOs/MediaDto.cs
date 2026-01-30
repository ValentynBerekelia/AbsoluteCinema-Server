using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Application.DTOs;

public record MediaDto
(
    Guid Id,
    MediaType MediaType,
    string Url
)
{
    
}