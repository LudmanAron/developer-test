using System.Xml.Serialization;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Infrastructure.Strategies
{
    /// <summary>
    /// Germany Strategy
    /// </summary>
    public class GermanyVatRegistrationStrategy : IVatRegistrationStrategy
    {
        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
            serializer.Serialize(stringWriter, request);
            var xml = stringWriter.ToString();
            var xmlQueueClient = new TaxuallyQueueClient();
            await xmlQueueClient.EnqueueAsync("vat-registration-xml", xml);
        }
    }
}
