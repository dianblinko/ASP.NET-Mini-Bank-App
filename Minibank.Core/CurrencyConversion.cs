using Minibank.Core.Domains;

namespace Minibank.Core
{
    public class CurrencyConversion : ICurrencyConversion
    {
        private readonly IExchangeRateSource _ExchangeRateSource;

        public CurrencyConversion(IExchangeRateSource ExchangeRatesSourse)
        {
            _ExchangeRateSource = ExchangeRatesSourse;
        }

        public double Converting(double amount, CurrencyEnum fromCurrency, CurrencyEnum toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("Отрицательная сумма");
            }

            double fromValuteCourse = _ExchangeRateSource.GetValuteCourse(fromCurrency);
            double toValuteCourse = _ExchangeRateSource.GetValuteCourse(toCurrency);

            double amountInRub = fromCurrency == CurrencyEnum.RUB ? amount :
                amount * fromValuteCourse;
            return toCurrency == CurrencyEnum.RUB ? amountInRub :
                amountInRub / toValuteCourse;
        }
    }
}
