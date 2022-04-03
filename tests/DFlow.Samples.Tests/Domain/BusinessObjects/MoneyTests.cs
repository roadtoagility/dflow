using Xunit;
using DFlow.Samples.Domain.BusinessObjects;
using DFlow.Samples.Tests.Domain.BusinessObjects;

namespace DFlow.Samples.Tests
{
    public class MoneyTests
    {
        private static readonly ICurrencyLookup _currencyLookup = new FakeCurrencyLookup();

        public class When_Money_Is_Valid : MoneyTests
        {
            [Fact]
            public void Zero_should_return_money()
            {
                var money = Money.Zero("EUR", _currencyLookup);

                Assert.NotNull(money);
                Assert.NotNull(money.Currency);
                Assert.Equal("EUR", money.Currency.Code);
                Assert.Equal(decimal.Zero, money.Value);
            }

            [Fact]
            public void FromDecimal_should_return_money()
            {
                var money = Money.FromDecimal(1.0m, "EUR", _currencyLookup);

                Assert.NotNull(money);
                Assert.NotNull(money.Currency);
                Assert.Equal("EUR", money.Currency.Code);
                Assert.Equal(1.0m, money.Value);
            }

            [Fact]
            public void FromString_should_return_money()
            {
                var money = Money.FromString("1.0", "EUR", _currencyLookup);

                Assert.NotNull(money);
                Assert.NotNull(money.Currency);
                Assert.Equal("EUR", money.Currency.Code);
                Assert.Equal(1.0m, money.Value);
            }


            [Fact]
            public void Add_should_return_money()
            {
                var money1 = Money.FromString("1.0", "EUR", _currencyLookup);
                var money2 = Money.FromString("0.2", "EUR", _currencyLookup);

                var sum = money1.Add(money2);

                Assert.NotNull(sum);
                Assert.NotNull(sum.Currency);
                Assert.Equal("EUR", sum.Currency.Code);
                Assert.Equal(1.2m, sum.Value);
            }

            [Fact]
            public void Substract_should_return_money()
            {
                var money1 = Money.FromString("1.0", "EUR", _currencyLookup);
                var money2 = Money.FromString("0.2", "EUR", _currencyLookup);

                var sum = money1.Substract(money2);

                Assert.NotNull(sum);
                Assert.NotNull(sum.Currency);
                Assert.Equal("EUR", sum.Currency.Code);
                Assert.Equal(0.8m, sum.Value);
            }
        }

        public class When_Money_Is_Invalid : MoneyTests
        {
            [Fact]
            public void From_should_return_money_given_no_currency()
            {
                var money = Money.From(1.0m);

                Assert.NotNull(money);
                Assert.Null(money.Currency);
            }

            [Fact]
            public void Add_should_throw_CurrencyException_given_no_currency()
            {
                var money1 = Money.From(1.0m);
                var money2 = Money.From(1.0m);

                Assert.Throws<CurrencyException>(() => money1 + money2);
            }

            [Fact]
            public void Substract_should_throw_CurrencyException_given_no_currency()
            {
                var money1 = Money.From(1.0m);
                var money2 = Money.From(1.0m);

                Assert.Throws<CurrencyException>(() => money1 - money2);
            }
        }
    }
}
