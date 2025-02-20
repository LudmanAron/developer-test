using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Core.Exceptions
{
    public class UnsupportedCountryException : VatRegistrationException
    {
        public UnsupportedCountryException(CountryCode country)
            : base($"VAT registration is not supported for country: {country}") { }
    }
}
