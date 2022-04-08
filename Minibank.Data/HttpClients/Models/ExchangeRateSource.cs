using Minibank.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Minibank.Core.Domains;

namespace Minibank.Data.HttpClients.Models
{
    public class ExchangeRateSource : IExchangeRateSource
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetValuteCourse(CurrencyEnum code)
        {
            var response = await _httpClient.GetFromJsonAsync<CourseResponse>("daily_json.js");

            if (code == CurrencyEnum.RUB)
            {
                return 1.0;
            }
            if (!response.Valute.ContainsKey(code.ToString()))
            {
                throw new ValidationException("Неправильный код валюты");
            }
            
            return (double)response.Valute[code.ToString()].Value;
        }
    }
}
