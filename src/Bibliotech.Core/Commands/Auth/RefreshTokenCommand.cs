using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Commands.Auth;

public record RefreshTokenCommand(
          string RefreshToken,
          string IpAddress
) : ICommand<Result<RefreshTokenResponse>>;


public record RefreshTokenResponse(
          string AccessToken,
          string RefreshToken,
          DateTime ExpiresAt,
          UserDto User
);
