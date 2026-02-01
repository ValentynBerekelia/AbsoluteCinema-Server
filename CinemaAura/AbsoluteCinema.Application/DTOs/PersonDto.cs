using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Application.DTOs;

public record PersonDto
(
    Guid PersonId,
    string PersonName,
    PersonRole PersonRole,
    string? ImageUrl
){}