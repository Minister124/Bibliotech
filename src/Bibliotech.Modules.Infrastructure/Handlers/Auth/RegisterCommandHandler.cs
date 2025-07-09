using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result<RegisterResponse>>
{

          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<RegisterCommandHandler> _logger;

          public RegisterCommandHandler(IAuthenticationService authenticationService, ILogger<RegisterCommandHandler> logger)
          {
                    _authenticationService = authenticationService;
                    _logger = logger;
          }

          public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing register command for email: {Email}", request.Email);

                    try
                    {
                              var authResult = await _authenticationService.RegisterAsync(
                                request.Email,
                                request.Password,
                                request.FirstName,
                                request.LastName
                              );

                              if (!authResult.Success)
                              {
                                        _logger.LogWarning("Registration failed for email: {Email}", request.Email);
                                        return Result<RegisterResponse>.Failure(authResult.Errors.ToArray());
                              }

                              var response = new RegisterResponse("Registration Successful. Please verify your email", authResult.User);
                              _logger.LogInformation("Registration successful for email: {Email}", request.Email);
                              return Result<RegisterResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error processing register command for email: {Email}", request.Email);
                              return Result<RegisterResponse>.Failure("An error occured during registration");
                    }
          }
}
