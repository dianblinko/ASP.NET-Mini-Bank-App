using System.Threading.Tasks;
using Minibank.Core.Domains;

namespace Minibank.Core
{
    public interface ICurrencyConversion
    {
        Task<double> Converting(double amount, CurrencyEnum fromCurrency, CurrencyEnum toCurrency);
    }
}
