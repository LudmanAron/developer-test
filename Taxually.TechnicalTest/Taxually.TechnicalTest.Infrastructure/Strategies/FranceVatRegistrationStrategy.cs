using System.Text;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Infrastructure.Strategies
{
    /// <summary>
    /// France strategry
    /// </summary>
    public class FranceVatRegistrationStrategy : IVatRegistrationStrategy
    {
        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            var excelQueueClient = new TaxuallyQueueClient();
            await excelQueueClient.EnqueueAsync("vat-registration-csv", csv);
        }
    }
}
