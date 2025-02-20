using System.Text;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Core.Clients;

namespace Taxually.TechnicalTest.Infrastructure.Strategies
{
    /// <summary>
    /// France strategry
    /// </summary>
    public class FranceVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly ITaxuallyQueueClient _queueClient;

        public FranceVatRegistrationStrategy(ITaxuallyQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            await _queueClient.EnqueueAsync("vat-registration-csv", csv);
        }
    }
}
