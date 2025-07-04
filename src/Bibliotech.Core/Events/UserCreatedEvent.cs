using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.ValueObjects;

namespace Bibliotech.Core.Events;

public record UserCreatedEvent(UserId UserId, string Email, string FirstName, string LastName) : IDomainEvent
{
          public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
