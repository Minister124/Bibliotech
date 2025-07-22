using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, Result<LogoutResponse>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IAuthenticationService authenticationService,
        ILogger<LogoutCommandHandler> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

          public async Task<Result<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing logout command");

                    try
                    {
                              var result = await _authenticationService.RevokeTokenAsync(
                              request.RefreshToken,
                              request.IpAddress);

                              if (!result)
                              {
                              _logger.LogWarning("Logout failed - token revocation unsuccessful");
                              return Result<LogoutResponse>.Failure("Failed to revoke token");
                              }

                              var response = new LogoutResponse("Logged out successfully");

                              _logger.LogInformation("Logout successful");
                              return Result<LogoutResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error processing logout command");
                              return Result<LogoutResponse>.Failure("An error occurred during logout");
                    }
          }
}