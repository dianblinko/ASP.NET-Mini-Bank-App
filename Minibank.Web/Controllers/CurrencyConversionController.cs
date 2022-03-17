using Microsoft.AspNetCore.Mvc;
using Minibank.Core;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route(template: "[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyСonversion _currencyConversion;

        public CurrencyConversionController(ICurrencyСonversion currencyСonversion)
        {
            _currencyConversion = currencyСonversion;
        }

        [HttpGet]
        public double Get(double amount, string fromCurrnecy, string toCurrency)
        {
            return _currencyConversion.Converting(amount, fromCurrnecy, toCurrency);
        }
    }
}
