using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.Commands.Auth;

public record LogoutCommand(
          string RefreshToken,
          string IpAddress
) : ICommand<Result<LogoutResponse>>;

public record LogoutResponse(
          string Message
);
