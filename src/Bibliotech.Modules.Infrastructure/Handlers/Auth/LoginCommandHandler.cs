using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginResponse>>
{

          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<LoginCommandHandler> _logger;

          public LoginCommandHandler(IAuthenticationService authenticationService, ILogger<LoginCommandHandler> logger)
          {
                    _authenticationService = authenticationService;
                    _logger = logger;
          }
          public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing login command for email: {Email}", request.Email);

                    try
                    {
                              var authResult = await _authenticationService.LoginAsync(
                                        request.Email,
                                        request.Password,
                                        request.IpAddress,
                                        request.DeviceInfo
                              );

                              if (!authResult.Success)
                              {
                                        _logger.LogWarning("Login failed for email: {Email}", request.Email);
                                        return Result<LoginResponse>.Failure(authResult.Errors.ToArray());
                              }
                              
                              var response = new LoginResponse(
                                        authResult.AccessToken,
                                        authResult.RefreshToken,
                                        authResult.ExpiresAt,
                                        authResult.User
                              );

                              _logger.LogInformation("Login successful for email: {Email}", request.Email);
                              return Result<LoginResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error processing login command for email: {Email}", request.Email);
                              return Result<LoginResponse>.Failure("An error occurred while processing your request");
                    }
          }
}
