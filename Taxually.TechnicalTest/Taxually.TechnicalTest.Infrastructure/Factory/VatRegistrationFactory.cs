using Taxually.TechnicalTest.Core.Exceptions;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Infrastructure.Strategies;

namespace Taxually.TechnicalTest.Infrastructure.Factory
{
    /// <summary>
    /// Factory to get the correct Strategy
    /// </summary>
    public class VatRegistrationFactory
    {
        private readonly Dictionary<CountryCode, IVatRegistrationStrategy> _strategies;

        public VatRegistrationFactory()
        {
            _strategies = new Dictionary<CountryCode, IVatRegistrationStrategy>
            {
            { CountryCode.GB, new UkVatRegistrationStrategy() },
            { CountryCode.FR, new UkVatRegistrationStrategy() },
            { CountryCode.DE, new GermanyVatRegistrationStrategy() }
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
