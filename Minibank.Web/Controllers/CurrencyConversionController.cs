using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core;
using Minibank.Core.Domains;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route(template: "[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyConversion _currencyConversion;

        public CurrencyConversionController(ICurrencyConversion currencyСonversion, CancellationToken cancellationToken)
        {
            _currencyConversion = currencyСonversion;
        }

        [HttpGet]
        public Task<double> Get(double amount, CurrencyEnum fromCurrnecy, CurrencyEnum toCurrency, 
            CancellationToken cancellationToken)
        {
            return _currencyConversion.Converting(amount, fromCurrnecy, toCurrency);
        }
    }
}
