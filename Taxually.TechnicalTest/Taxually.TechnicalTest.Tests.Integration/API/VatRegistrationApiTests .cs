using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Taxually.TechnicalTest.Core.Models;

namespace Taxually.TechnicalTest.Tests.Integration.API
{
    public class VatRegistrationApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VatRegistrationApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_RegisterVat_ShouldReturnOk_ForGB()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.GB,
                CompanyName = "TestCo",
                CompanyId = "123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/vat/register", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_RegisterVat_ShouldReturnBadRequest_ForUnsupportedCountry()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.Unknown,
                CompanyName = "TestCo",
                CompanyId = "123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/vat/register", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

}
