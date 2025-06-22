using System;

namespace Bibliotech.Core.Models;

public class AuthenticationResult
{
          public bool Success { get; set; }
          public string AccessToken {get; set;} = string.Empty;
          public string RefreshToken {get; set;} = string.Empty;
          public DateTime ExpiresAt {get; set;}
          public UserDto User {get; set;} = null!;
          public List<string> Errors {get; set;} = new();
}

public class UserDto
{
          public string Id { get; set; } = string.Empty;
          public string Email { get; set; } = string.Empty;
          public string FirstName { get; set; } = string.Empty;
          public string LastName { get; set; } = string.Empty;
          public bool IsEmailVerified { get; set; }
          public List<string> Roles { get; set; } = new();
}
