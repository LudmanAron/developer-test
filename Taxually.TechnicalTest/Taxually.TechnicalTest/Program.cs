using AspNetCoreRateLimit;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Polly;

using Taxually.TechnicalTest.API.Extensions;
using Taxually.TechnicalTest.API.Middlewares;
using Taxually.TechnicalTest.Core.Clients;
using Taxually.TechnicalTest.Core.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Azure Key Vault
        var keyVaultName = builder.Configuration["KeyVaultName"];
        var kvUri = $"https://{keyVaultName}.vault.azure.net/";
        builder.Configuration.AddAzureKeyVault(new Uri(kvUri), new DefaultAzureCredential());

        builder.Services.AddClients();
        builder.Services.AddFactories();
        builder.Services.AddServices();

        // Register response compression services
        builder.Services.AddResponseCompression();

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add Application Insights
        builder.Services.AddApplicationInsightsTelemetry();

        // Add Response Caching
        builder.Services.AddResponseCaching();

        // Add Authentication (optional, replace with your setup)
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.Authority = "<Your-Auth-Server>";
                            options.Audience = "<Your-API-Audience>";
                        });

        // Add Logging
        builder.Services.AddLogging(options =>
        {
            options.AddConsole();
            options.AddDebug();
            options.AddApplicationInsights();
        });

        // Add Rate Limiting (optional)
        builder.Services.AddMemoryCache();
        builder.Services.AddInMemoryRateLimiting();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        var app = builder.Build();

        // Configure middleware
        app.UseMiddleware<RequestLoggingMiddleware>(); // Useful for debugging

        // Unnecessary now, since the controller already catches all potential exceptions, but could be useful for the future
        //app.UseMiddleware<ExceptionHandlingMiddleware>(); 

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseResponseCompression(); // Compress responses for better performance
        app.UseAuthentication();     // Authenticate requests
        app.UseAuthorization();      // Authorize requests

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}