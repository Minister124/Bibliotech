using System;
using MediatR;

namespace Bibliotech.Modules.Infrastructure.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
          public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
          {
                    throw new NotImplementedException();
          }
}
