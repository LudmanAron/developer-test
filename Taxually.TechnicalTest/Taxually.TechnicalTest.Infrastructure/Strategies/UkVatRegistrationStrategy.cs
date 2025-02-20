using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Infrastructure.Strategies
{
    /// <summary>
    /// UK Strategy
    /// </summary>
    public class UkVatRegistrationStrategy : IVatRegistrationStrategy
    {
        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            var httpClient = new TaxuallyHttpClient();
            await httpClient.PostAsync("https://api.uktax.gov.uk", request);
        }
    }
}
