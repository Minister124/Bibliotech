using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.Commands.Auth;

public record VerifyEmailCommand(
          string Token
) : ICommand<Result<VerifyEmailResponse>>;

public record VerifyEmailResponse(
          string Message
);
