using Polly;
using Taxually.TechnicalTest.Core.Clients;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Infrastructure.Factory;
using Taxually.TechnicalTest.Infrastructure.Services;

namespace Taxually.TechnicalTest.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddScoped<ITaxuallyQueueClient, TaxuallyQueueClient>();
            services.AddHttpClient<ITaxuallyHttpClient, TaxuallyHttpClient>()
                    .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3)) // Retry 3 times on transient errors
                    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); // Circuit breaker with 30s timeout

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IVatRegistrationService, VatRegistrationService>();

            return services;
        }

        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddScoped<IVatRegistrationFactory, VatRegistrationFactory>();
            
            return services;
        }
    }
}
