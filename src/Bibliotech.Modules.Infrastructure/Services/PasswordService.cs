using System;
using BCrypt.Net;
using Bibliotech.Core.Services;

namespace Bibliotech.Modules.Infrastructure.Services;

public class PasswordService : IPasswordService
{
          private const int MinPasswordLength = 8;
          private const int WorkFactor = 12;

          public string HashPassword(string password)
          {
                    return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
          }

          public bool IsValidPassword(string password)
          {
                    if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength) return false;

                    var hasUpperCase = password.Any(char.IsUpper);
                    var hasLowerCase = password.Any(char.IsLower);
                    var hasDigit = password.Any(char.IsDigit);
                    var hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

                    return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
          }

          public bool VerifyPassword(string password, string hash)
          {
                    return BCrypt.Net.BCrypt.Verify(password, hash);
          }
}
