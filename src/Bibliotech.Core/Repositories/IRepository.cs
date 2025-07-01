using System;
using System.Linq.Expressions;
using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.Repositories;

public interface IRepository<TEntity, TId> where TEntity : Entity<TId>
{
          Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
          Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
          Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
          Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
          Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
          Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
          Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
          Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
          Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
}

public interface IAggregateRepository<TAggregate, TId> : IRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
          // Additional methods specific to aggregates can be added here
}
