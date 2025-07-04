using System;

namespace Bibliotech.Core.Abstractions;

public abstract class ValueObject : IEquatable<ValueObject>
{

          protected abstract IEnumerable<object?> GetEqualityComponents();
          public bool Equals(ValueObject? other)
          {
                    if (other is null) return false;
                    if (ReferenceEquals(this, other)) return true;
                    if (GetType() != other.GetType()) return false;

                    return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
          }

          public override bool Equals(object? obj)
          {
                    return Equals(obj as ValueObject);
          }

          public override int GetHashCode()
          {
                    return GetEqualityComponents().Where(x => x != null)
                              .Aggregate(1, (current, obj) => current * 23 + obj!.GetHashCode());
          }

          public static bool operator ==(ValueObject? left, ValueObject? right)
          {
                    return Equals(left, right);
          }

          public static bool operator !=(ValueObject? left, ValueObject? right)
          {
                    return !Equals(left, right);
          }
}
