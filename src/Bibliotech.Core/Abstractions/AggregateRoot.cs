using System;

namespace Bibliotech.Core.Abstractions;

public abstract class AggregateRoot<T> : Entity<T>
{
          private readonly List<IDomainEvent> _domainEvents = new();

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

public abstract class AuditableAggregateRoot<T> : AuditableEntity<T>
{
          private readonly List<IDomainEvent> _domainEvents = new();

          public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

          protected void AddDomainEvent(IDomainEvent domainEvent)
          {
                    _domainEvents.Add(domainEvent);
          }

          public void ClearDomainEvents()
          {
                    _domainEvents.Clear();
          }

          protected void MarkAsModified(string? updatedBy = null)
          {
                    MarkAsUpdated(updatedBy);
          }
}

public interface IDomainEvent
{
          DateTime OccurredOn {get;}
}
