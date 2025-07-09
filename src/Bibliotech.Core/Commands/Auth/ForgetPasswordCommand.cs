using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.Commands.Auth;

public record ForgetPasswordCommand(
          string Email
) : ICommand<Result<ForgetPasswordResponse>>;

public record ForgetPasswordResponse(
          string Message
);
