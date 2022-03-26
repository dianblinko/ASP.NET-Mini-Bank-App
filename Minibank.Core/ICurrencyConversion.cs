using Minibank.Core.Domains;

namespace Minibank.Core
{
    public interface ICurrencyConversion
    {
        double Converting(double amount, CurrencyEnum fromCurrency, CurrencyEnum toCurrency);
    }
}
