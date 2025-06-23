using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Bibliotech.Core.Configuration;
using Bibliotech.Core.Entities;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Bibliotech.Modules.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
          private readonly JwtSettings _jwtSettings;
          private readonly ILogger<JwtTokenService> _logger;

          public JwtTokenService(IOptions<JwtSettings> jwtSettings, ILogger<JwtTokenService> logger)
          {
                    _jwtSettings = jwtSettings.Value;
                    _logger = logger;
          }

          public string GenerateAccessToken(User user)
          {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                    var claims = new List<Claim>
                    {
                              new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
                              new(ClaimTypes.Email, user.Email),
                              new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                              new("firstName", user.FirstName),
                              new("lastName", user.LastName),
                              new("emailVerified", user.IsEmailVerified.ToString()),
                              new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                              new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                    };

                    foreach (var role in user.Roles)
                    {
                              claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                              Subject = new ClaimsIdentity(claims),
                              Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationDays),
                              Issuer = _jwtSettings.Issuer,
                              Audience = _jwtSettings.Audience,
                              SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
          }

          public string GenerateRefreshToken()
          {
                    var randomBytes = new byte[64];
                    using var rng = RandomNumberGenerator.Create();
                    rng.GetBytes(randomBytes);
                    return Convert.ToBase64String(randomBytes);

          }

          public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
          {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                    var validationParameters = new TokenValidationParameters
                    {
                              ValidateIssuer = _jwtSettings.ValidateIssuer,
                              ValidateAudience = _jwtSettings.ValidateAudience,
                              ValidateLifetime = false, //not validating lifetime for refresh
                              ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                              IssuerSigningKey = new SymmetricSecurityKey(key),
                              ValidIssuer = _jwtSettings.Issuer,
                              ValidAudience = _jwtSettings.Audience,
                              ClockSkew = TimeSpan.Zero
                    };

                    try
                    {
                              var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                              if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                              {
                                        return null;
                              }

                              return principal;
                    }
                    catch (Exception ex)
                    {
                              _logger.LogWarning(ex, "Failed to validate expired token");
                              return null;
                    }
          }

          public bool ValidateToken(string token)
          {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                    var validationParameters = new TokenValidationParameters
                    {
                              ValidateIssuer = _jwtSettings.ValidateIssuer,
                              ValidateAudience = _jwtSettings.ValidateAudience,
                              ValidateLifetime = _jwtSettings.ValidateLifetime,
                              ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                              ValidIssuer = _jwtSettings.Issuer,
                              ValidAudience = _jwtSettings.Audience,
                              IssuerSigningKey = new SymmetricSecurityKey(key),
                              ClockSkew = TimeSpan.Zero
                    };

                    try
                    {
                              tokenHandler.ValidateToken(token, validationParameters, out _);
                              return true;
                    }
                    catch
                    {
                              return false;
                    }
          }
}
