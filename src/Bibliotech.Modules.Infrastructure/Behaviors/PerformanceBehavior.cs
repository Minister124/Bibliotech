using System;
using System.Diagnostics;
using Amazon.Runtime.Internal.Util;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

          private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
          private readonly int _slowRequestThresholdMs;

          public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
          {
                    _logger = logger;
                    _slowRequestThresholdMs = 500;
          }

          public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
          {
                    var stopwatch = Stopwatch.StartNew();

                    var response = await next();

                    stopwatch.Stop();

                    if (stopwatch.ElapsedMilliseconds > _slowRequestThresholdMs)
                    {
                              var requestName = typeof(TRequest).Name;
                              _logger.LogWarning("Slow request detected: {RequestName} took {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);
                    }

                    return response;
          }
}
