using Minibank.Core.Domains;

namespace Minibank.Core
{
    public interface IExchangeRateSource
    {
        double GetValuteCourse(CurrencyEnum code);
    }
}
