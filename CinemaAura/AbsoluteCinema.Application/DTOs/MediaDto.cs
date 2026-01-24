using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Application.DTOs;

public record MediaDto
(
    MediaType MediaType,
    string Url
)
{
    
}