using System.Security.Claims;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Bibliotech.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new {Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)});

            var command = new RegisterCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName
            );

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Errors = result.Errors });

            if (result.Value == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Unexpected error occurred." });

            return Ok(new
            {
                Message = result.Value.Message,
                User = result.Value.User
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var ipAddress = GetIpAddress();
            var deviceInfo = GetDeviceInfo();

            var command = new LoginCommand(
                request.Email,
                request.Password,
                ipAddress,
                deviceInfo
            );

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new {Errors = result.Errors});

            if (result.Value == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Unexpected error occurred." });

            SetRefreshTokenCookie(result.Value.RefreshToken);

            return Ok(new
            {
                AccessToken = result.Value.AccessToken,
                ExpiresAt = result.Value.ExpiresAt,
                User = result.Value.User
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { Error = "Refresh token is required" });

            var command = new RefreshTokenCommand(refreshToken, GetIpAddress());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new {Errors = result.Errors});

            if (result.Value == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Unexpected error occurred." });

            SetRefreshTokenCookie(result.Value.RefreshToken);

            return Ok(new
            {
                AccessToken = result.Value.AccessToken,
                ExpiresAt = result.Value.ExpiresAt,
                User = result.Value.User
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Error = "Refresh token is required" });
            }
            else
            {
                var command = new LogoutCommand(refreshToken, GetIpAddress());
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                    return BadRequest(new {Errors = result.Errors});

                if (result.Value == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Unexpected error occurred." });

                Response.Cookies.Delete("refreshToken");

                return Ok(new
                {
                    Message = "Logged out successfully"
                });
            }
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var command = new VerifyEmailCommand(request.Token);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { Message = result.Value.Message });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordRequest request)
        {
            var command = new ForgetPasswordCommand(request.Email);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { Message = result.Value.Message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request.Token, request.NewPassword);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { Message = result.Value.Message });
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
    public record ResetPasswordRequest(string Token, string NewPassword);
}
