using DFlow.Domain.BusinessObjects;

namespace DFlow.Samples.Domain.BusinessObjects
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string code);
    }

    public sealed class Currency : ValueOf<string, Currency>
    {
        public bool IsEnabled { get; set; }

        public int DecimalDigits { get; set; }

        public string Code { get; set; }

        public static Currency None { get => From(string.Empty); }
    }
}