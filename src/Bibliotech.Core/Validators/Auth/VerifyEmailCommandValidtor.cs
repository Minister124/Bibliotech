using System;
using Bibliotech.Core.Commands.Auth;
using FluentValidation;

namespace Bibliotech.Core.Validators.Auth;

public class VerifyEmailCommandValidtor : AbstractValidator<VerifyEmailCommand>
{
          public VerifyEmailCommandValidtor()
          {
                    RuleFor(x => x.Token)
                              .NotEmpty().WithMessage("Token is required");
          }
}
