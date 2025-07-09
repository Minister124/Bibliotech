using System;
using Bibliotech.Core.Commands.Auth;
using FluentValidation;

namespace Bibliotech.Core.Validators.Auth;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
          public RegisterCommandValidator()
          {
                    RuleFor(x => x.Email)
                               .NotEmpty().WithMessage("Email is required")
                               .EmailAddress().WithMessage("Invalid email format")
                               .MaximumLength(255).WithMessage("Email must not excced 255 characters");
                    RuleFor(x => x.Password)
                              .NotEmpty().WithMessage("Password is required")
                              .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                              .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                              .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
                    RuleFor(x => x.FirstName)
                              .NotEmpty().WithMessage("First name is required")
                              .MaximumLength(100).WithMessage("First name must not exceed 100 characters")
                              .Matches(@"^[a-zA-Z\s]+$").WithMessage("First name can only contain letters and spaces");

                    RuleFor(x => x.LastName)
                              .NotEmpty().WithMessage("Last name is required")
                              .MaximumLength(100).WithMessage("Last name must not exceed 100 characters")
                              .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last name can only contain letters and spaces");
          }
}
