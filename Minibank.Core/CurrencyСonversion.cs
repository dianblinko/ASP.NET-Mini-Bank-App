using System;

namespace Minibank.Core
{
    public class CurrencyСonversion : ICurrencyСonversion
    {
        private readonly IExchangeRates _ExchangeRates;
        public CurrencyСonversion(IExchangeRates ExchangeRates)
        {
            _ExchangeRates = ExchangeRates; 
        }
        public int Converting(int sum, string code)
        {
            var value = _ExchangeRates.Get(code);
            if (value * sum < 0)
            {
                throw new UserFriendlyException("Отрицательная сумма");
            }
            else
            {
                return value * sum;
            }
            
        }
        
    }
}
