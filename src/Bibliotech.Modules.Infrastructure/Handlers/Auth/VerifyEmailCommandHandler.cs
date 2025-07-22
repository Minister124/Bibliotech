using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Bibliotech.Modules.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, Result<VerifyEmailResponse>>
{

          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<VerifyEmailCommand> _logger;

          public VerifyEmailCommandHandler(IAuthenticationService authenticationService, ILogger<VerifyEmailCommand> logger)
          {
                    _authenticationService = authenticationService;
                    _logger = logger;
          }
          public async Task<Result<VerifyEmailResponse>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing logout command");

                    try
                    {
                              var result = await _authenticationService.VerifyEmailAsync(
                                        request.Token
                              );

                              if (!result)
                              {
                                        _logger.LogWarning("Email verification failed, Invalid token");
                                        return Result<VerifyEmailResponse>.Failure("Invalid verification token");
                              }

                              var response = new VerifyEmailResponse("Email verified successfully");
                              _logger.LogInformation("Email verification successful");
                              return Result<VerifyEmailResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error processing email verification command");
                              return Result<VerifyEmailResponse>.Failure("An error occurred while processing your request");
                    }
          }
}
