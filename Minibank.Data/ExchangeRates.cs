using Minibank.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data
{
    public class ExchangeRates : IExchangeRates
    {
        Random random;
        public ExchangeRates()
        {
            random = new Random();
        }
        public int Get(string code)
        {
            return random.Next(1,150);
        }

    }
}
