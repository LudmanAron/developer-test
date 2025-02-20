using System.Xml.Serialization;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Core.Clients;

namespace Taxually.TechnicalTest.Infrastructure.Strategies
{
    /// <summary>
    /// Germany Strategy
    /// </summary>
    public class GermanyVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly ITaxuallyQueueClient _queueClient;

        public GermanyVatRegistrationStrategy(ITaxuallyQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task RegisterVatAsync(VatRegistrationRequest request)
        {
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
            serializer.Serialize(stringWriter, request);
            var xml = stringWriter.ToString();
            await _queueClient.EnqueueAsync("vat-registration-xml", xml);
        }
    }
}
