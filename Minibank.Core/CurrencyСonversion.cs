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

            double fromValuteCourse = _ExchangeRateSource.GetValuteCourse(fromCurrency);
            double toValuteCourse = _ExchangeRateSource.GetValuteCourse(toCurrency);

            double amountInRub = fromCurrency == "RUB" ? amount :
                amount * fromValuteCourse;

            return toCurrency == "RUB" ? amountInRub :
                amountInRub / toValuteCourse;
        }
    }
}
