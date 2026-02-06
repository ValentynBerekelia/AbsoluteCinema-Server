using AbsoluteCinema.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AbsoluteCinema.Infrastructure.Security;

public sealed class RequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _http;

    public RequestContext(IHttpContextAccessor http)
    {
        _http = http;
    }

    public string IpAddress =>
        _http.HttpContext?.Connection.RemoteIpAddress?.ToString()
        ?? "unknown";

    public string? UserAgent =>
        _http.HttpContext?.Request.Headers.UserAgent.ToString();

    public Guid? UserId
    {
        get
        {
            var id = _http.HttpContext?.User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(id, out var guid) ? guid : null;
        }
    }
}