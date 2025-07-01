using System;
using MediatR;

namespace Bibliotech.Core.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{}
