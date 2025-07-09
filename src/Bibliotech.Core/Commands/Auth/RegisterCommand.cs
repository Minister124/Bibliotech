using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Commands.Auth;

public record RegisterCommand(
          string Email,
          string Password,
          string FirstName,
          string LastName
) : ICommand<Result<RegisterResponse>>;

public record RegisterResponse(
          string Message,
          UserDto User
);
