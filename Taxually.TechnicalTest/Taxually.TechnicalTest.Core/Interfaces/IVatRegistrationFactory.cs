using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Core.Interfaces
{
    public interface IVatRegistrationFactory
    {
        IVatRegistrationStrategy GetStrategy(CountryCode country);
    }
}
