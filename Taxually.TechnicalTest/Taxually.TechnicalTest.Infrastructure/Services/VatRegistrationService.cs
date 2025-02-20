using Taxually.TechnicalTest.Core.Factory;
using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Infrastructure.Services
{
    public class VatRegistrationService
    {
        private readonly VatRegistrationFactory _factory;

        public VatRegistrationService(VatRegistrationFactory factory)
        {
            _factory = factory;
        }

        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            var strategy = _factory.GetStrategy(request.Country);
            await strategy.RegisterVatAsync(request);
        }
    }
}
