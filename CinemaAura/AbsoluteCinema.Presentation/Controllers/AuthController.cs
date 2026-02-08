using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Auth;
using AbsoluteCinema.Application.Features.Auth.Command.LoginUser;
using AbsoluteCinema.Application.Features.Auth.Command.Logout;
using AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;
using AbsoluteCinema.Application.Features.Auth.Command.RevokeAllRefreshTokens;
using AbsoluteCinema.Application.Features.Auth.Queries.GetCurrentUser;
using AbsoluteCinema.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

[Route("api/auth")]
public class AuthController(IMediator mediator, IRequestContext requestContext) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IRequestContext _requestContext = requestContext;

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpGet("debug/claims")]
    public IActionResult DebugClaims()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterUserRequest request)
    {
        var command = request.Adapt<CreateUserCommand>();
        var response = await _mediator.Send(command);
        
        Response.Cookies.Append(
            "refreshToken",
            response.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(14),
            });
        
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginUserRequest request)
    {
        var command = request.Adapt<LoginUserCommand>();
        var response = await _mediator.Send(command);
        
        Response.Cookies.Append(
            "refreshToken",
            response.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(14),
            });
        
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { error = "Refresh token not found." });
        }

        var command = new RefreshTokenCommand { RefreshToken = refreshToken };
        var response = await _mediator.Send(command);
        
        Response.Cookies.Append(
            "refreshToken",
            response.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(14),
            });
        
        return Ok(response);
    }
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken) && !string.IsNullOrEmpty(refreshToken))
        {
            var command = new LogoutCommand { RefreshToken = refreshToken };
            await _mediator.Send(command);
        }

        Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/auth" });
        return Ok();
    }

    [Authorize]
    [HttpPost("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var userId = _requestContext.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new RevokeAllRefreshTokensCommand { UserId = userId.Value };
        await _mediator.Send(command);
        return Ok();
    }
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = _requestContext.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        var response = await _mediator.Send(new GetCurrentUserQuery(userId.Value));
        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }
}