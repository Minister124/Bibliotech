using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Commands.Auth;
using Bibliotech.Core.Services;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Handlers.Auth;

public class ForgetPasswordCommandHandler : ICommandHandler<ForgetPasswordCommand, Result<ForgetPasswordResponse>>
{

          private readonly IAuthenticationService _authenticationService;
          private readonly ILogger<ForgetPasswordCommandHandler> _logger;

          public ForgetPasswordCommandHandler(IAuthenticationService authenticationService, ILogger<ForgetPasswordCommandHandler> logger)
          {
                    _authenticationService = authenticationService;
                    _logger = logger;
          }

          public async Task<Result<ForgetPasswordResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
          {
                    _logger.LogInformation("Processing forget password command for email: {Email}", request.Email);

                    try
                    {
                              var result = await _authenticationService.ForgetPasswordAsync(request.Email);

                              if (!result)
                              {
                                        _logger.LogWarning("Forget password failed for email: {Email}", request.Email);
                                        return Result<ForgetPasswordResponse>.Failure("Failed to process password reset request");
                              }

                              var response = new ForgetPasswordResponse("Password reset request sent");
                              _logger.LogInformation("Forget password successful for email: {Email}", request.Email);
                              return Result<ForgetPasswordResponse>.Success(response);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError("Error processing forget password command");
                              return Result<ForgetPasswordResponse>.Failure("An error occurred while processing your request");
                    }
          }
}
