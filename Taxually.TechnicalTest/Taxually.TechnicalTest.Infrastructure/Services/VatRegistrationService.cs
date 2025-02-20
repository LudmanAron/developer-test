using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Infrastructure.Services
{
    public class VatRegistrationService : IVatRegistrationService
    {
        private readonly IVatRegistrationFactory _factory;

        public VatRegistrationService(IVatRegistrationFactory factory)
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
