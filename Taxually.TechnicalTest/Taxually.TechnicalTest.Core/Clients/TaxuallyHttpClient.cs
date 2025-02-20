using Taxually.TechnicalTest.Core.Interfaces;

namespace Taxually.TechnicalTest.Core.Clients
{
    public class TaxuallyHttpClient : ITaxuallyHttpClient
    {
        public Task PostAsync<TRequest>(string url, TRequest request)
        {
            // Actual HTTP call removed for purposes of this exercise
            return Task.CompletedTask;
        }
    }
}
