using System;
using System.Globalization;
using DFlow.Domain.BusinessObjects;

namespace DFlow.Samples.Domain.BusinessObjects
{
    public sealed class Money : ValueOf<decimal, Money>
    {
        public Currency Currency { get; private set; }

        public Money()
        {
        }

        private Money(decimal value, string currencyCode, ICurrencyLookup currencyLookup)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new ArgumentException($"{ nameof(currencyCode) } is required");
            }

            var currency = currencyLookup.FindCurrency(currencyCode);

            if (!currency.IsEnabled)
            {
                throw new CurrencyException($"{ nameof(currencyCode) } is not enabled");
            }

            if (decimal.Round(value, currency.DecimalDigits) != value)
            {
                throw new ArgumentOutOfRangeException($"Amount in { currencyCode } must have { currency.DecimalDigits } as maximum");
            }

            Value = value;
            Currency = currency;
        }

        private Money(decimal value, Currency currency)
        {
            if (currency == null)
            {
                throw new CurrencyException($"Currency was not provided");
            }

            if (decimal.Round(value, currency.DecimalDigits) != value)
            {
                throw new ArgumentOutOfRangeException($"Amount in { currency.Code } must have { currency.DecimalDigits } as maximum");
            }

            Value = value;
            Currency = currency;
        }

        public static Money Zero(string currencyCode, ICurrencyLookup currencyLookup)
        {
            return new Money(decimal.Zero, currencyCode, currencyLookup);
        }

        public static Money FromDecimal(decimal value, string currencyCode, ICurrencyLookup currencyLookup)
        {
            return new Money(value, currencyCode, currencyLookup);
        }

        public static Money FromString(string value, string currencyCode, ICurrencyLookup currencyLookup)
        {
            var parsed = decimal.Parse(value, CultureInfo.InvariantCulture);
            return new Money(parsed, currencyCode, currencyLookup);
        }

        public static Money operator +(Money summand1, Money summand2) =>
            summand1.Add(summand2);

        public static Money operator -(Money minuend, Money subtrahend) =>
            minuend.Substract(subtrahend);

        public override string ToString() =>
            $"{Currency.Code} { Value }";

        public Money Add(Money summand)
        {
            if (Currency != summand.Currency)
            {
                throw new CurrencyException("Currencies don't match");
            }

            return new Money(Value + summand.Value, Currency);
        }

        public Money Substract(Money subtrahend)
        {
            if (Currency != subtrahend.Currency)
            {
                throw new CurrencyException("Currencies don't match");
            }

            if (subtrahend.Value > Value)
            {
                throw new ArgumentOutOfRangeException("Subtrahen is greater than current Value");
            }

            return new Money(Value - subtrahend.Value, Currency);
        }
    }

    public class CurrencyException : Exception
    {
        public CurrencyException(string message) : base(message)
        {
        }
    }
}