using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Core.Clients;

namespace Taxually.TechnicalTest.Infrastructure.Strategies
{
    /// <summary>
    /// UK Strategy
    /// </summary>
    public class UkVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly ITaxuallyHttpClient _httpClient;

        public UkVatRegistrationStrategy(ITaxuallyHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            await _httpClient.PostAsync("https://api.uktax.gov.uk", request);
        }
    }
}
