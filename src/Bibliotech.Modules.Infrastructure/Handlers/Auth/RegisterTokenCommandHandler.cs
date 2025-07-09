using System;
using Amazon.Runtime.Internal.Util;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class RegisterTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<RegisterCommandHandler> _logger;
          public Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
          {
                    throw new NotImplementedException();
          }
}
