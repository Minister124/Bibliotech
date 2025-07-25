using System;

namespace Bibliotech.Core.Abstractions;

public abstract record DomainEvent
{
          public DateTime OccurredOn { get; } = DateTime.UtcNow;
          public Guid EventID { get; } = Guid.NewGuid();
}
