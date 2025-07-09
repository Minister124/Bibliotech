using System;
using Bibliotech.Core.Commands.Auth;
using FluentValidation;

namespace Bibliotech.Core.Validators.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
          public LoginCommandValidator()
          {
                    RuleFor(x => x.Email)
                              .NotEmpty().WithMessage("Email is required")
                              .EmailAddress().WithMessage("Invalid email format")
                              .MaximumLength(255).WithMessage("Email must not excced 255 characters");
                    RuleFor(x => x.Password)
                              .NotEmpty().WithMessage("Password is required")
                              .MinimumLength(1).WithMessage("Password is required");
                    RuleFor(x => x.IpAddress)
                              .NotEmpty().WithMessage("IP Address is required");
                    RuleFor(x => x.DeviceInfo)
                              .NotEmpty().WithMessage("Device Info is required")
                              .MaximumLength(500).WithMessage("Device info must not exceed 500 characters");
          }
}
