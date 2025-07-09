using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.Commands.Auth;

public record ResetPasswordCommand(
          string Token,
          string NewPassword
) : ICommand<Result<ResetPasswordResponse>>;

public record ResetPasswordResponse(
          string Message
);
