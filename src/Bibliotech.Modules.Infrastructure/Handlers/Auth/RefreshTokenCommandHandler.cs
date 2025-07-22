using System;
using Amazon.Runtime.Internal.Util;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<RegisterCommandHandler> _logger;

          public RefreshTokenCommandHandler(IAuthenticationService authenticationService, ILogger<RegisterCommandHandler> logger)
          {
                    _authenticationService = authenticationService;
                    _logger = logger;
          }
          public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing refresh token commnad");

                    try
                    {
                              var authResult = await _authenticationService.RefreshTokenAsync(
                                        request.RefreshToken,
                                        request.IpAddress
                              );

                              if (!authResult.Success)
                              {
                                        _logger.LogWarning("Refresh token failed)");
                                        return Result<RefreshTokenResponse>.Failure(authResult.Errors.ToArray());
                              }

                              var response = new RefreshTokenResponse(
                                        authResult.AccessToken,
                                        authResult.RefreshToken,
                                        authResult.ExpiresAt,
                                        authResult.User
                              );

                              _logger.LogInformation("Refresh token successful");
                              return Result<RefreshTokenResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error processing refresh token command");
                              return Result<RefreshTokenResponse>.Failure("An error occurred while processing your request");
                    }
          }
}
