using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.ValueObjects;

namespace Bibliotech.Core.Events;

public record UserRoleAddedEvent(UserId UserId, string Role) : IDomainEvent
{
          public DateTime OccurredOn { get; } = DateTime.UtcNow;
}