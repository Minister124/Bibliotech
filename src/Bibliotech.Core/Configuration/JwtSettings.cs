using System;

namespace Bibliotech.Core.Configuration;

public class JwtSettings
{
          public const string SectionName = "JwtSettings";
          public string SecretKey { get; set; } = string.Empty;
          public string Issuer { get; set; } = string.Empty;
          public string Audience { get; set; } = string.Empty;
          public int AccessTokenExpirationMinutes { get; set; } = 15;
          public int RefreshTokenExpirationDays { get; set; } = 7;
          public bool ValidateIssuer { get; set; } = true;
          public bool ValidateAudience { get; set; } = true;
          public bool ValidateLifetime { get; set; } = true;
          public bool ValidateIssuerSigningKey { get; set; } = true;
}
