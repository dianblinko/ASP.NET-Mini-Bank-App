using Minibank.Core;
using Microsoft.AspNetCore.Mvc;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route(template:"[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyСonversion _currencyConversion;

        public CurrencyConversionController(ICurrencyСonversion currencyСonversion)
        {
            _currencyConversion = currencyСonversion;
        }

        [HttpGet]
        public int Get(int sum, string code)
        {
            return _currencyConversion.Converting(sum, code);
        }
    }
}
