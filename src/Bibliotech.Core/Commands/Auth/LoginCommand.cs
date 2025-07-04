using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Commands.Auth;

public record LoginCommand(
          string Email,
          string Password,
          string IpAddress,
          string DeviceInfo
) : ICommand<Result<LoginResponse>>;
public record LoginResponse(
          string AccessToken,
          string RefreshToken,
          DateTime ExpiresAt,
          UserDto User
);         
