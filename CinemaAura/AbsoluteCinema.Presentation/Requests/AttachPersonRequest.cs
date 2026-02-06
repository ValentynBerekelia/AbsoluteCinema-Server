using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests
{
    /// <summary>
    /// Request for attaching existing person to movie
    /// </summary>
    public record AttachPersonRequest(Guid PersonId, PersonRole Role);
}
