using System;

namespace Minibank.Core
{
    public class CurrencyСonversion : ICurrencyСonversion
    {
        private readonly IExchangeRateSource _ExchangeRateSource;

        public CurrencyСonversion(IExchangeRateSource ExchangeRatesSourse)
        {
            _ExchangeRateSource = ExchangeRatesSourse; 
        }
        public int Converting(int sum, string code)
        {
            if (sum < 0) 
            {
                throw new UserFriendlyException("Отрицательная сумма");
            }
            else
            {
                var value = _ExchangeRateSource.Get(code);
                return value * sum;
            }
        }
    }
}
