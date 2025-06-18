using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.ValueObjects;

namespace Bibliotech.Core.Events;

public record UserEmailVerifiedEvent(UserId UserId, string Email) : IDomainEvent
{
          public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
