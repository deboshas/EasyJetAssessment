using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EasyJet.UnitTest
{
    public class EasyJet
    {
        private readonly MoneyCalculator _moneyCalculator;     
        public EasyJet()
        {
            _moneyCalculator = new MoneyCalculator();           
        }

        [Theory] 
        [InlineData("GBP 10, GBP 20, GBP 30")]
        [InlineData("GBP 10, GBP 50, GBP 30")]
        [InlineData("GBP 10, GBP 20, GBP 60")]
        [InlineData("GBP 90, GBP 20, GBP 30")]
        public void TestAllMoneyAreinSameCurrency_ReturnsTrue(string currencyData)
        {
            var moniesInSameCurrency = GetMoneyList(currencyData);
            Assert.True(moniesInSameCurrency.IsEqualCurrency());         
        }

        [Theory]
        [InlineData("GBP 10, GBP 20, gbP 40")]
        [InlineData("GBP 10, GBP 50, gbp 20")]
        [InlineData("gbP 10, GBP 20, GBP 60")]
        [InlineData("GBP 90, GBp 20, GBP 30")]
        public void TestAllMoneisAreinSameCurrencyWithDifferentCase_ReturnsFasle(string currencyData)
        {
            var moniesInDifferentCurrency = GetMoneyList(currencyData);
            Assert.False(moniesInDifferentCurrency.IsEqualCurrency());
        }

        [Theory]
        [InlineData("GBP 10, EUR 20, GBP 40")]
        [InlineData("USD 10, GBP 50, GBP 20")]
        [InlineData("AUD 10, GBP 20, GBP 60")]
        [InlineData("GBP 90, AUD 20, GBP 30")]
        public void TestAllMoneyAreNotinSameCurrency_ReturnsFalse(string currencyData)
        {
            var moniesInDifferentCurrency = GetMoneyList(currencyData);
            Assert.False(moniesInDifferentCurrency.IsEqualCurrency());
        }

        [Theory]
        [InlineData("GBP 10, GBP 20, GBP 30",30)]
        [InlineData("GBP 10, GBP 50, GBP 30",50)]
        [InlineData("GBP 10, GBP 20, GBP 60",60)]
        [InlineData("GBP 90, GBP 20, GBP 30",90)]
        public void TestMaxOfMoniesWithinSameCurrency_ReturnsMaxAmount(string currencyData,decimal maxAmount)
        {
            var monies = GetMoneyList(currencyData);          
            var deriedMaxMoney = _moneyCalculator.Max(monies);
            Assert.Equal(deriedMaxMoney.Amount, maxAmount);          
        }

        [Theory]
        [InlineData("GBP 10, USD 20, GBP 30")]
        [InlineData("EUR 10, GBP 50, GBP 30")]
        [InlineData("GBP 10, GBP 20, CAD 60")]
        [InlineData("INR 90, GBP 20, GBP 30")]
        public void TestMaxOfMoniesHavingDifferentCurrency_ThrowsArgumentException(string currencyData)
        {
            var monies = GetMoneyList(currencyData);
            var moneyCalculator = new MoneyCalculator();          
            Assert.Throws<ArgumentException>(() => moneyCalculator.Max(monies));
        }

        [Theory]
        [InlineData("GBP 10, USD 20, GBP 30", "GBP 40,USD 20")]
        [InlineData("EUR 10, GBP 50, GBP 30", "GBP 80, EUR 10")]
        [InlineData("GBP 10, GBP 20, CAD 60", "GBP 30, CAD 60")]
        [InlineData("INR 90, GBP 20, GBP 30", "INR 90, GBP 50")]
        public void TestSumPerCurrency_ReturnsSumofAmount(string currencyData, string sumPerCurrency)
        {
            var monies = GetMoneyList(currencyData);
            var sumPerCurrencyCalculated = GetMoneyList(sumPerCurrency);
            var deriedSumPerCurrency = _moneyCalculator.SumPerCurrency(monies);           
            foreach (var money in sumPerCurrencyCalculated)
            {
                Assert.NotNull(deriedSumPerCurrency.Where(x=>x.Currency.Equals(money.Currency)).FirstOrDefault());
                Assert.Equal(money.Amount, deriedSumPerCurrency.Where(x => x.Currency.Equals(money.Currency)).FirstOrDefault().Amount);
            }
            
        }
   
        [Theory]
        [InlineData("")]
        public void TestSumPerCurrencyWithEmptyMoney_ReturnsEmptyCollection(string currencyData)
        {
            var monies = GetMoneyList(currencyData);           
            Assert.Empty(_moneyCalculator.SumPerCurrency(monies));
        }

        private IEnumerable<IMoney> GetMoneyList(string currencyData)
        {
            decimal amount;
            var monies = new List<IMoney>();
            if (!string.IsNullOrEmpty(currencyData))
            {
                var currencyItems = currencyData.Split(",");
                if ((currencyItems?.Any()).Value)
                {
                    foreach (var item in currencyItems)
                    {
                        var money = item.Trim().Split(" ");
                        if (money.Count() == 2)
                        {
                            decimal.TryParse(money[1], out amount);
                            monies.Add(new Money()
                            {
                                Currency = money[0],
                                Amount = amount
                            }); ;
                        }
                    }
                }
            }
            return monies;

        }
    }
}
