using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository;

public interface IMediaRepository : IRepository<MediaId, Media>
{
}
