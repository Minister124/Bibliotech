using System;
using System.Security.Claims;
using Bibliotech.Core.Entities;

namespace Bibliotech.Core.Services;

public interface IJwtTokenService
{
          string GenerateAccessToken(User user);
          string GenerateRefreshToken();
          ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
          bool ValidateToken(string token);
}
