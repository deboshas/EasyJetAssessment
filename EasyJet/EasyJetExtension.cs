using System.Collections.Generic;
using System.Linq;

namespace EasyJet
{
    public static class EasyJetExtension
    {/// <summary>
    /// Check monies are in same currency or not
    /// </summary>
    /// <param name="monies"></param>
    /// <returns></returns>
        public static bool IsEqualCurrency(this IEnumerable<IMoney> monies)
        {
            return !(monies.ToList().GroupBy(x => x.Currency).Count() > 1);
        }
    }
}
