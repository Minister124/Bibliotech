using System;
using Bibliotech.Core.Commands.Auth;
using FluentValidation;

namespace Bibliotech.Core.Validators.Auth;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
          public RefreshTokenCommandValidator()
          {
                    RuleFor(x => x.RefreshToken)
                              .NotEmpty().WithMessage("Refresh token is required");
                    RuleFor(x => x.IpAddress)
                              .NotEmpty().WithMessage("IP Address is required");
          }
}
