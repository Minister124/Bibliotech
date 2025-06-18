using System;

namespace Bibliotech.Core.Abstractions;

public interface IEntity<T>
{
          T Id { get; }
}

public interface IAuditableEntity
{
          DateTime CreatedAt {get;}
          DateTime? UpdatedAt {get;}
          string? CreatedBy {get;}
          string? UpdatedBy {get;}
}
