using System.Threading.Tasks;
using Minibank.Core.Domains;

namespace Minibank.Core
{
    public interface IExchangeRateSource
    {
        Task<double> GetValuteCourse(CurrencyEnum code);
    }
}
