using System;

namespace Bibliotech.Core.Abstractions;

public abstract class DomainException : Exception
{
          public string ErrorCode { get; }
          public object? Details { get; }

          protected DomainException(string message, string errorCode, object? details = null)
                : base(message)
          {
                    ErrorCode = errorCode;
                    Details = details;
          }

          protected DomainException(string message, string errorCode, Exception innerException, object? details = null)
                : base(message, innerException)
          {
                    ErrorCode = errorCode;
                    Details = details;
          }
}

public class BusinessRuleValidationException : DomainException
{
          public BusinessRuleValidationException(string message, string errorCode = "BUSINESS_RULE_VIOLATION", object? details = null) : base(message, errorCode, details)
          {
          }
}

public class EntityNotFoundException : DomainException
{
          public EntityNotFoundException(string entityName, object id)
                    : base($"{entityName} with id '{id}' was not found", "ENTITY_NOT_FOUND", new { EntityName = entityName, Id = id })
          {
          }
}
