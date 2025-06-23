using System;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Services;

public interface IAuthenticationService
{
          Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName);
          Task<AuthenticationResult> LoginAsync(string email, string password, string ipAddress, string deviceInfo);
          Task<AuthenticationResult> RefreshTokenAsync(string refresgToken, string ipAddress);
          Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress);
          Task<bool> VerifyEmailAsync(string token);
          Task<bool> ForgetPasswordAsync(string email);
          Task<bool> ResetPasswordAsync(string token, string newPassword);
}
