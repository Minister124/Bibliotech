using System;
using Bibliotech.Core.Configuration;
using Bibliotech.Core.Entities;
using Bibliotech.Core.Models;
using Bibliotech.Core.Repositories;
using Bibliotech.Core.Services;
using Bibliotech.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bibliotech.Modules.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
          private readonly IUserRepository _userRepository;
          private readonly IRefreshTokenRepository _refreshToken;
          private readonly IJwtTokenService _jwtTokenService;
          private readonly IPasswordService _passwordService;
          private readonly JwtSettings _jwtSettings;
          private readonly ILogger<AuthenticationService> _logger;

          public AuthenticationService(
                    IUserRepository userRepository,
                    IRefreshTokenRepository refreshToken,
                    IJwtTokenService jwtTokenService,
                    IPasswordService passwordService,
                    IOptions<JwtSettings> jwtSettings,
                    ILogger<AuthenticationService> logger
          )
          {
                    _userRepository = userRepository;
                    _refreshToken = refreshToken;
                    _jwtTokenService = jwtTokenService;
                    _passwordService = passwordService;
                    _jwtSettings = jwtSettings.Value;
                    _logger = logger;
          }

          public async Task<bool> ForgetPasswordAsync(string email)
          {
                    var user = await _userRepository.GetByEmailAsync(email);

                    if (user == null) return false;

                    user.resetPasswordToken();
                    await _userRepository.UpdateAsync(user);

                    //TODO: Send email with reset token

                    return true;
          }

          public async Task<AuthenticationResult> LoginAsync(string email, string password, string ipAddress, string deviceInfo)
          {
                    var user = await _userRepository.GetByEmailAsync(email);

                    if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
                    {
                              _logger.LogWarning("Failed Login attempt for email: {Email}", email);
                              return new AuthenticationResult
                              {
                                        Success = false,
                                        Errors = new List<string>
                                        {
                                                  "Invalid email or password"
                                        }
                              };
                    }

                    if (user.Status != UserStatus.Active)
                    {
                              return new AuthenticationResult
                              {
                                        Success = false,
                                        Errors = new List<string>
                                        {
                                                  "Account is not Active"
                                        }
                              };
                    }

                    var accessToken = _jwtTokenService.GenerateAccessToken(user);
                    var refreshToken = _jwtTokenService.GenerateRefreshToken();

                    var refreshTokenEntity = new RefreshToken
                    {
                              UserId = user.Id.Value.ToString(),
                              Token = refreshToken,
                              ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                              DeviceInfo = deviceInfo,
                              IpAddress = ipAddress
                    };

                    await _refreshToken.AddAsync(refreshTokenEntity);

                    user.UpdateLastActiveDate();
                    await _userRepository.UpdateAsync(user);

                    _logger.LogInformation("User logged in successfully: {Email}", email);

                    return new AuthenticationResult
                    {
                              Success = true,
                              AccessToken = accessToken,
                              RefreshToken = refreshToken,
                              ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                              User = MapToUserDto(user)
                    };
          }

          public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken, string ipAddress)
          {
                    var storedToken = await _refreshToken.GetByTokenAsync(refreshToken);

                    if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate <= DateTime.UtcNow)
                    {
                              return new AuthenticationResult
                              {
                                        Success = false,
                                        Errors = new List<string>
                                        {
                                                  "Invalid refresh token or expired refresh token"
                                        }
                              };
                    }

                    var user = await _userRepository.GetByIdAsync(UserId.From(Guid.Parse(storedToken.UserId)));
                    if (user == null || user.Status != UserStatus.Active)
                    {
                              return new AuthenticationResult
                              {
                                        Success = false,
                                        Errors = new List<string>
                                        {
                                                  "User not found or account is not active"
                                        }
                              };
                    }

                    var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
                    var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

                    storedToken.IsRevoked = true;
                    storedToken.RevokedAt = DateTime.UtcNow;
                    storedToken.ReplaceByToken = newRefreshToken;
                    await _refreshToken.UpdateAsync(storedToken);

                    var newRefreshTokenEntity = new RefreshToken
                    {
                              UserId = user.Id.Value.ToString(),
                              Token = newRefreshToken,
                              ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                              DeviceInfo = storedToken.DeviceInfo,
                              IpAddress = ipAddress
                    };

                    await _refreshToken.AddAsync(newRefreshTokenEntity);

                    return new AuthenticationResult
                    {
                              Success = true,
                              AccessToken = newAccessToken,
                              RefreshToken = newRefreshToken,
                              ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                              User = MapToUserDto(user)
                    };
          }

          public async Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName)
          {
                    if (!_passwordService.IsValidPassword(password))
                    {
                              return new AuthenticationResult
                              {
                                        Success = false,
                                        Errors = new List<string>
                                        {
                                                  "Password does not meet security requirements"
                                        }
                              };
                    }

                    if (await _userRepository.ExistsByEmailAsync(email))
                    {
                              return new AuthenticationResult
                              {
                                        Success = false,
                                        Errors = new List<string>
                                        {
                                                  "User with this email already exists"
                                        }
                              };
                    }

                    var passwordHash = _passwordService.HashPassword(password);
                    var user = User.Create(email, passwordHash, firstName, lastName);
                    if (await _userRepository.AnyUsers())
                    {
                              user.AddRole("User");
                    }
                    else
                    {
                              user.AddRole("Admin");
                    }

                    await _userRepository.AddAsync(user);

                    _logger.LogInformation("User registered successfully: {Email}", email);

                    return new AuthenticationResult
                    {
                              Success = true,
                              User = MapToUserDto(user)
                    };
          }

          public async Task<bool> ResetPasswordAsync(string token, string newPassword)
          {
                    if (!_passwordService.IsValidPassword(newPassword)) return false;

                    var user = await _userRepository.GetByPasswordResetTokenAsync(token);

                    if (user == null) return false;

                    var newPasswordHash = _passwordService.HashPassword(newPassword);

                    user.ResetPassword(newPasswordHash);
                    
                    await _userRepository.UpdateAsync(user);

                    await _refreshToken.RemoveAllUserTokenAsync(user.Id.Value.ToString());

                    return true;
          }

          public async Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress)
          {
                    var storedToken = await _refreshToken.GetByTokenAsync(refreshToken);

                    if (storedToken == null || storedToken.IsRevoked) return false;

                    storedToken.IsRevoked = true;
                    storedToken.RevokedAt = DateTime.UtcNow;

                    await _refreshToken.UpdateAsync(storedToken);

                    return true;
          }

          public async Task<bool> VerifyEmailAsync(string token)
          {
                    var user = await _userRepository.GetByEmailVerificationTokenAsync(token);

                    if (user == null) return false;

                    user.VerifyEmail();
                    await _userRepository.UpdateAsync(user);

                    return true;
          }

          private static UserDto MapToUserDto(User user)
          {
                    return new UserDto
                    {
                              Id = user.Id.Value.ToString(),
                              Email = user.Email,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              IsEmailVerified = user.IsEmailVerified,
                              Roles = user.Roles.ToList()
                    };
          }
}
