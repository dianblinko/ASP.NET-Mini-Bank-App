namespace Minibank.Core
{
    public interface ICurrencyСonversion
    {
        double Converting(double amount, string fromCurrency, string toCurrency);
    }
}
