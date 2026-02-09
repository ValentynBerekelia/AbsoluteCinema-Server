using System.Security.Cryptography.X509Certificates;
using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests
{
    /// <summary>
    /// Request body for creating new media
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="Type"></param>
    public record CreateMediaRequest(
        string? Url = null,
        IFormFile? File = null,
        MediaType Type = 0
    );
}
