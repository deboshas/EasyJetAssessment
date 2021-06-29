using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyJet
{
    public class Money : IMoney
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
    
    public class MoneyCalculator : IMoneyCalculator
    {
        /// <summary>
        /// Retunrs max of monies belongs to same currency
        /// </summary>
        /// <param name="monies"></param>
        /// <returns></returns>
        public IMoney Max(IEnumerable<IMoney> monies)
        { 
            if (monies.IsEqualCurrency())
            {
                return monies.ToList()
                              .GroupBy(m => m.Currency)
                              .Select(gr => new Money { Currency = gr.Key, Amount = gr.Max(m => m.Amount) }).FirstOrDefault();

            }
            else
            {
                throw new ArgumentException(Constants.EXCEPTIONMESSAGE);
            }

        }
        /// <summary>
        /// Returns sum per currency 
        /// </summary>
        /// <param name="monies"></param>
        /// <returns></returns>
        public IEnumerable<IMoney> SumPerCurrency(IEnumerable<IMoney> monies)
        {
            if (monies!=null && monies.Any())
            {
                return monies.ToList()
                              .GroupBy(m => m.Currency)
                              .Select(gr => new Money { Currency = gr.Key, Amount = gr.Sum(m => m.Amount) })
                              .OrderByDescending(m => m.Currency);



            }
            return monies;
        }       
    }
}
