using System;

namespace Bibliotech.Core.Abstractions;

public abstract class Entity<T> : IEntity<T>, IEquatable<Entity<T>>
{
          public T Id { get; protected set; } = default;

          protected Entity() { }

          protected Entity(T id)
          {
                    Id = id;
          }

          public bool Equals(Entity<T>? other)
          {
                    if (other is null)
                              return false;

                    if (ReferenceEquals(this, other))
                              return true;

                    if (GetType() != other.GetType())
                              return false;

                    return EqualityComparer<T>.Default.Equals(Id, other.Id);
          }

          public override bool Equals(object? obj)
          {
                    return Equals(obj as Entity<T>);
          }

          public override int GetHashCode()
          {
                    return Id?.GetHashCode() ?? 0;
          }

          public static bool operator ==(Entity<T>? left, Entity<T>? right)
          {
                    return Equals(left, right);
          }

          public static bool operator !=(Entity<T>? left, Entity<T>? right)
          {
                    return !Equals(left, right);
          }
}
