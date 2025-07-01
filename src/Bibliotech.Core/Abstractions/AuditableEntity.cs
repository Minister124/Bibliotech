using System;

namespace Bibliotech.Core.Abstractions;

public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
{
          public DateTime CreatedAt { get; protected set; }

          public DateTime? UpdatedAt { get; protected set; }

          public string? CreatedBy { get; protected set; }

          public string? UpdatedBy { get; protected set; }
          
          protected AuditableEntity() : base()
          {
                    CreatedAt = DateTime.UtcNow;
          }

          protected AuditableEntity(T id) : base(id)
          {
                    CreatedAt = DateTime.UtcNow;
          }

          public void SetAuditInfo(string? createdBy = null, string? updatedBy = null)
          {
                    if (CreatedBy == null && createdBy != null)
                    {
                              CreatedBy = createdBy;
                    }

                    if (updatedBy != null)
                    {
                              UpdatedBy = updatedBy;
                              UpdatedAt = DateTime.UtcNow;
                    }
          }

          protected void MarkAsUpdated(string? updatedBy  = null)
          {
                    UpdatedAt = DateTime.UtcNow;
                    if (updatedBy != null)
                              UpdatedBy = updatedBy;
          }
}
