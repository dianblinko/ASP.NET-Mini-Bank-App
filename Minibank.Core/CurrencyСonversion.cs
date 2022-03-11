using System;

namespace Minibank.Core
{
    public class CurrencyСonversion : ICurrencyСonversion
    {
        private readonly IExchangeRateSource _ExchangeRateSource;

        public CurrencyСonversion(IExchangeRateSource ExchangeRates)
        {
            _ExchangeRateSource = ExchangeRates; 
        }
        public int Converting(int sum, string code)
        {
            var value = _ExchangeRateSource.Get(code);
            if (sum < 0) 
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
