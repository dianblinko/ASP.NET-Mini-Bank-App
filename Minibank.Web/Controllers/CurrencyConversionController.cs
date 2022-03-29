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

        public CurrencyConversionController(ICurrencyConversion currencyСonversion)
        {
            _currencyConversion = currencyСonversion;
        }

        [HttpGet]
        public double Get(double amount, CurrencyEnum fromCurrnecy, CurrencyEnum toCurrency)
        {
            return _currencyConversion.Converting(amount, fromCurrnecy, toCurrency);
        }
    }
}
