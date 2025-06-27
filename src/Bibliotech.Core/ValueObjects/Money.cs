using System;
using Bibliotech.Core.Abstractions;

namespace Bibliotech.Core.ValueObjects;

public class Money : ValueObject
{

          public decimal Amount { get; }
          public string Currency { get; }

          private Money(decimal amount, string currency)
          {
                    Amount = amount;
                    Currency = currency;
          }

          public static Money Create(decimal amount, string currency = "USD")
          {
                    if (amount < 0)
                              throw new BusinessRuleValidationException("Money amount cannot be negative", "INVALID_MONEY_AMOUNT");

                    if (string.IsNullOrWhiteSpace(currency))
                              throw new BusinessRuleValidationException("Currency cannot be empty", "INVALID_CURRENCY");

                    return new Money(amount, currency.ToUpperInvariant());
          }

          public static Money Zero(string currency = "USD") => new(0, currency);

          public Money Add(Money other)
          {
                    if (Currency != other.Currency)
                              throw new BusinessRuleValidationException("Cannot add money with different currencies", "CURRENCY_MISMATCH");

                    return new Money(Amount + other.Amount, Currency);
          }

          public Money Subtract(Money other)
          {
                    if (Currency != other.Currency)
                              throw new BusinessRuleValidationException("Cannot subtract money with different currencies", "CURRENCY_MISMATCH");

                    var result = Amount - other.Amount;
                    if (result < 0)
                              throw new BusinessRuleValidationException("Money amount cannot be negative", "INVALID_MONEY_AMOUNT");

                    return new Money(result, Currency);
          }

          protected override IEnumerable<object?> GetEqualityComponents()
          {
                    yield return Amount;
                    yield return Currency;
          }

          public override string ToString() => $"{Amount:C} {Currency}";
}
