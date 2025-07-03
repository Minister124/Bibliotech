using System.Security.Claims;
using Bibliotech.Core.Services;
using Bibliotech.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthController> _logger;

        private readonly IMediator _mediator;

        public AuthController(IAuthenticationService authenticationService, ILogger<AuthController> logger, IMediator mediator)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName);

            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { Message = "Registration Successful, Please verify your email", User = result.User });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var ipAddress = GetIpAddress();
            var deviceInfo = GetDeviceInfo();

            var result = await _authenticationService.LoginAsync(
                request.Email, request.Password, ipAddress, deviceInfo
            );

            if (!result.Success) return BadRequest(new { Errors = result.Errors });

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(new
            {
                AccessToken = result.AccessToken,
                ExpiresAt = result.ExpiresAt,
                User = result.User
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { Error = "Refresh token is missing" });

            var ipAddress = GetIpAddress();
            var result = await _authenticationService.RefreshTokenAsync(refreshToken, ipAddress);

            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(new
            {
                AccessToken = result.AccessToken,
                ExpiresAt = result.ExpiresAt,
                User = result.User
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var ipAddress = GetIpAddress();
                await _authenticationService.RevokeTokenAsync(refreshToken, ipAddress);
            }

            Response.Cookies.Delete("refershToken");
            return Ok(new { Message = "Logged Out Successfully"});
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authenticationService.VerifyEmailAsync(request.Token);

            if (!result) return BadRequest(new { Error = "Invalid verification token" });

            return Ok(new { Message = "Email verified successfully" });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Roles = roles
            });
        }

        private string GetIpAddress()
        {
            return Request.Headers.ContainsKey("X-Forwarded-For") ? Request.Headers["X-Forwarded-For"].ToString() : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private string GetDeviceInfo()
        {
            return Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "Unknown";
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }

    public record RegisterRequest(string Email, string Password, string FirstName, string LastName);
    public record LoginRequest(string Email, string Password);
    public record VerifyEmailRequest(string Token);
}
