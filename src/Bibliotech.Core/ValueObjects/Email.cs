using System;
using System.Text.RegularExpressions;
using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.ValueObjects;

public class Email : ValueObject
{

          private static readonly Regex EmailRegex = new(
                    @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

          public string Value { get; }

          private Email(string value)
          {
                    Value = value;
          }

          public static Email Create(string email)
          {
                    if (string.IsNullOrWhiteSpace(email))
                              throw new BusinessRuleValidationException("Email cannot be empty", "INVALID_EMAIL");

                    email = email.Trim().ToLowerInvariant();

                    if (!EmailRegex.IsMatch(email))
                              throw new BusinessRuleValidationException("Invalid email format", "INVALID_EMAIL");

                    return new Email(email);
          }
          protected override IEnumerable<object?> GetEqualityComponents()
          {
                    yield return Value;
          }

          public static implicit operator string(Email email) => email.Value;

          public override string ToString() => Value;
}
