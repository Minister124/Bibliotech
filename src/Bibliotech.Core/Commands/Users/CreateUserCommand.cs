using System.Windows.Input;
using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.Commands.Users;

public record CreateUserCommand(
          string Email,
          string Password,
          string FirstName,
          string LastName) : ICommand<Result<CreateUserResponse>>;

public record CreateUserResponse(
          string UserId,
          string Email,
          string FirstName,
          string LastName,
          DateTime CreatedAt
);
