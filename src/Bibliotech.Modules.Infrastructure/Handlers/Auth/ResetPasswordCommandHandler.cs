using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
{

          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<ResetPasswordCommandHandler> _logger;

          public ResetPasswordCommandHandler(IAuthenticationService authenticationService, ILogger<ResetPasswordCommandHandler> logger)
          {
                    _authenticationService = authenticationService;
                    _logger = logger;
          }

          public async Task<Result<ResetPasswordResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing reset password command");

                    try
                    {
                              var result = await _authenticationService.ResetPasswordAsync(request.Token, request.NewPassword);

                              if (!result)
                              {
                                        _logger.LogWarning("Reset password failed");
                                        return Result<ResetPasswordResponse>.Failure("Failed to reset password");
                              }

                              var response = new ResetPasswordResponse("Password reset successfully");
                              _logger.LogInformation("Reset password successful");
                              return Result<ResetPasswordResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error processing reset password command");
                              return Result<ResetPasswordResponse>.Failure("An error occurred while processing your request");
                    }
          }
}
