using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Commands.Users;

public record GetUserByIdQuery(string UserId) : IQuery<Result<UserDto>>;

public record UserDto(
          string Id,
          string Email,
          string FirstName,
          string LastName,
          bool IsEmailVerified,
          List<string> Roles
);
