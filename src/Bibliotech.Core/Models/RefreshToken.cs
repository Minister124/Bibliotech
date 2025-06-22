using System;

namespace Bibliotech.Core.Models;

public class RefreshToken
{
          public string Id { get; set; } = Guid.NewGuid().ToString();
          public string UserId { get; set; } = string.Empty;
          public string Token { get; set; } = string.Empty;
          public DateTime ExpiryDate { get; set; }
          public bool IsRevoked { get; set; }
          public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
          public string? RevokedBy {get; set;}
          public DateTime? RevokedAt {get; set;}
          public string? ReplaceByToken {get; set;}
          public string? DeviceInfo {get; set;} = string.Empty;
          public string? IpAddress {get; set;} = string.Empty;
}
