using System;

namespace Bibliotech.Core.Abstractions;

public abstract class AggregateRoot<T> : IEntity<T>
{
          private readonly List<IDomainEvent> _domainEvents = new();

          public T Id { get; protected set; } = default!;

          public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

          protected void AddDomainEvent(IDomainEvent domainEvent)
          {
                    _domainEvents.Add(domainEvent);
          }

          public void ClearDomainEvents()
          {
                    _domainEvents.Clear();
          }
}

public interface IDomainEvent
{
          DateTime OccurredOn {get;}
}
