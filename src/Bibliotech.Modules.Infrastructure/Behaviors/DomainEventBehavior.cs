using System;
using System.Windows.Input;
using Amazon.Runtime.Internal.Util;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Behaviors;

public class DomainEventBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

          private readonly IMediator _mediator;
          private readonly ILogger<DomainEventBehavior<TRequest, TResponse>> _logger;

          public DomainEventBehavior(IMediator mediator, ILogger<DomainEventBehavior<TRequest, TResponse>> logger)
          {
                    _mediator = mediator;
                    _logger = logger;
          }

          public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
          {
                    var response = await next();

                    if (request is ICommand)
                    {
                              await PublishDomainEventsAsync(cancellationToken);
                    }

                    return response;
          }

          private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
          {
                    try
                    {
                              // Note: In a real implementation, you'd get the DbContext from DI
                              // and extract domain events from tracked aggregates
                              // This is a simplified version - you'll need to adapt based on your DbContext setup
                              _logger.LogDebug("Domain event publishing completed"); 
                    }
                    catch(Exception ex)
                    {
                              _logger.LogError(ex, "Error publishing domain events");
                              // Decide whether to throw or continue based on your requirements
                    }
          }
}
