using System;
using Bibliotech.Core.Commands.Auth;
using FluentValidation;

namespace Bibliotech.Core.Validators.Auth;

public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
          public ForgetPasswordCommandValidator()
          {
                    RuleFor(x => x.Email)
                              .NotEmpty().WithMessage("Email is required")
                              .EmailAddress().WithMessage("Invalid email format")
                              .MaximumLength(255).WithMessage("Email must not excced 255 characters");
          }
}
