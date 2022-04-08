using System.Threading.Tasks;
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

        public async Task<double> Converting(double amount, CurrencyEnum fromCurrency, CurrencyEnum toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("Отрицательная сумма");
            }

            double fromValuteCourse = await _ExchangeRateSource.GetValuteCourse(fromCurrency);
            double toValuteCourse = await _ExchangeRateSource.GetValuteCourse(toCurrency);

            double amountInRub = fromCurrency == CurrencyEnum.RUB ? amount :
                amount * fromValuteCourse;
            return toCurrency == CurrencyEnum.RUB ? amountInRub :
                amountInRub / toValuteCourse;
        }
    }
}
