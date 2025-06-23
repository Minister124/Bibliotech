using System;

namespace Bibliotech.Core.Services;

public interface IPasswordService
{
          string HashPassword(string password);
          bool VerifyPassword(string password, string hash);
          bool IsValidPassword(string password);
}
