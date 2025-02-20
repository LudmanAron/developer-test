using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Core.Interfaces
{
    public interface IVatRegistrationService
    {
        Task RegisterVatAsync(VatRegistrationRequest request);
    }
}
