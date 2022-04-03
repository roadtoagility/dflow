using System.Collections.Generic;
using System.Linq;
using DFlow.Samples.Domain.BusinessObjects;

namespace DFlow.Samples.Tests.Domain.BusinessObjects
{
    public class FakeCurrencyLookup : ICurrencyLookup
    {
        static readonly IEnumerable<Currency> _currencies;

        static FakeCurrencyLookup()
        {
            _currencies =
                            new[]
            {
                new Currency
                {
                    Code = "EUR",
                    DecimalDigits = 2,
                    IsEnabled = true
                },
                new Currency
                {
                    Code = "USD",
                    DecimalDigits = 2,
                    IsEnabled = true
                },
                new Currency
                {
                    Code = "GNF",
                    DecimalDigits = 0,
                    IsEnabled = true
                },
                new Currency
                {
                    Code = "ZZZ",
                    DecimalDigits = 2,
                    IsEnabled = false
                }
            };
        }

        public Currency FindCurrency(string code) =>
            _currencies.FirstOrDefault(c => c.Code == code) ?? Currency.None;
    }
}