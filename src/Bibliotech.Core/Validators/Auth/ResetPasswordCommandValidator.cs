using System;
using Bibliotech.Core.Commands.Auth;
using FluentValidation;

namespace Bibliotech.Core.Validators.Auth;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
          public ResetPasswordCommandValidator()
          {
                    RuleFor(x => x.Token)
                              .NotEmpty().WithMessage("Token is required");
                    RuleFor(x => x.NewPassword)
                              .NotEmpty().WithMessage("New password is required")
                              .MinimumLength(8).WithMessage("New password must be at least 8 characters")
                              .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                              .WithMessage("New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
          }
}
