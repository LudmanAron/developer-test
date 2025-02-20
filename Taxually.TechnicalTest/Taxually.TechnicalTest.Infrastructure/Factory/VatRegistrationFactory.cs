using Taxually.TechnicalTest.Core.Exceptions;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Infrastructure.Strategies;

namespace Taxually.TechnicalTest.Infrastructure.Factory
{
    /// <summary>
    /// Factory to get the correct Strategy
    /// </summary>
    public class VatRegistrationFactory : IVatRegistrationFactory
    {
        private readonly Dictionary<CountryCode, IVatRegistrationStrategy> _strategies;
        private readonly ITaxuallyQueueClient _queueClient;
        private readonly ITaxuallyHttpClient _httpClient;

        public VatRegistrationFactory(ITaxuallyQueueClient queueClient, ITaxuallyHttpClient httpClient)
        {
            _queueClient = queueClient;
            _httpClient = httpClient;
        }

        public VatRegistrationFactory()
        {
            _strategies = new Dictionary<CountryCode, IVatRegistrationStrategy>
            {
            { CountryCode.GB, new UkVatRegistrationStrategy(_httpClient) },
            { CountryCode.FR, new FranceVatRegistrationStrategy(_queueClient) },
            { CountryCode.DE, new GermanyVatRegistrationStrategy(_queueClient) }
        };
        }

        public IVatRegistrationStrategy GetStrategy(CountryCode country)
        {
            if (_strategies.TryGetValue(country, out var strategy))
            {
                return strategy;
            }

            throw new UnsupportedCountryException(country);
        }
    }
}
