using System;
using System.Threading.Tasks;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result)
            return BadRequest(new { Success = false, Message = "Username is already taken." });

        return Ok(new { Success = true, Message = "Registration successful." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var ipAddress = GetIpAddress();
        var response = await _authService.LoginAsync(request, ipAddress);

        if (response == null)
            return Unauthorized(new { Success = false, Message = "Invalid username or password." });

        SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpiration);

        return Ok(new { Success = true, Data = response });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto? request)
    {
        var token = request?.Token ?? Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { Success = false, Message = "Refresh token is required." });

        var ipAddress = GetIpAddress();
        var response = await _authService.RefreshTokenAsync(token, ipAddress);

        if (response == null)
            return Unauthorized(new { Success = false, Message = "Invalid or expired refresh token." });

        SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpiration);

        return Ok(new { Success = true, Data = response });
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto? request)
    {
        var token = request?.Token ?? Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { Success = false, Message = "Refresh token is required." });

        var ipAddress = GetIpAddress();
        var result = await _authService.RevokeTokenAsync(token, ipAddress);

        if (!result)
            return NotFound(new { Success = false, Message = "Token not found or already inactive." });

        return Ok(new { Success = true, Message = "Token revoked successfully." });
    }

    private void SetRefreshTokenCookie(string token, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires,
            Secure = true,
            SameSite = SameSiteMode.None
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"]!;
        
        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";
    }
}
