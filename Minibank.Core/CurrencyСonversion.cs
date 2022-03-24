namespace Minibank.Core
{
    public class CurrencyСonversion : ICurrencyСonversion
    {
        private readonly IExchangeRateSource _ExchangeRateSource;

        public CurrencyСonversion(IExchangeRateSource ExchangeRatesSourse)
        {
            _ExchangeRateSource = ExchangeRatesSourse;
        }

        public double Converting(double amount, string fromCurrency, string toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("Отрицательная сумма");
            }

            double amountInRub = fromCurrency == "RUB" ? amount :
                amount * _ExchangeRateSource.GetValuteCourse(fromCurrency);

            return toCurrency == "RUB" ? amountInRub :
                amountInRub / _ExchangeRateSource.GetValuteCourse(toCurrency);
        }
    }
}
