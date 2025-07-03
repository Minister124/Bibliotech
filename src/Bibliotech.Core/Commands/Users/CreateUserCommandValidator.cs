using System;
using Bibliotech.Core.Commands.Users;
using FluentValidation;

namespace Bibliotech.Core.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
          public CreateUserCommandValidator()
          {
                    RuleFor(x => x.Email)
                              .NotEmpty()
                              .WithMessage("Email is required")
                              .EmailAddress()
                              .WithMessage("Invalid email format");

                    RuleFor(x => x.Password)
                              .NotEmpty()
                              .WithMessage("Password is required")
                              .MinimumLength(8)
                              .WithMessage("Password must be at least 8 characters long")
                              .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                              .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character");

                    RuleFor(x => x.FirstName)
                              .NotEmpty()
                              .WithMessage("First name is required")
                              .MaximumLength(50)
                              .WithMessage("First name must not exceed 50 characters");

                    RuleFor(x => x.LastName)
                              .NotEmpty()
                              .WithMessage("Last name is required")
                              .MaximumLength(50)
                              .WithMessage("Last name must not exceed 50 characters");
          }
}
