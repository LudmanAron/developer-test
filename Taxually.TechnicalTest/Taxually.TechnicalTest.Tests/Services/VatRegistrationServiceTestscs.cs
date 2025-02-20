using Xunit;
using Moq;
using Taxually.TechnicalTest.Infrastructure.Services;
using Taxually.TechnicalTest.Core.Models;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Exceptions;

namespace Taxually.TechnicalTest.Tests.Unit.Services
{

    public class VatRegistrationServiceTests
    {
        private readonly Mock<IVatRegistrationFactory> _mockFactory;
        private readonly Mock<ITaxuallyQueueClient> _mockQueueClient;
        private readonly Mock<ITaxuallyHttpClient> _mockHttpClient;
        private readonly VatRegistrationService _service;

        public VatRegistrationServiceTests()
        {
            _mockFactory = new Mock<IVatRegistrationFactory>();
            _mockQueueClient = new Mock<ITaxuallyQueueClient>();
            _mockHttpClient = new Mock<ITaxuallyHttpClient>();

            // Setting up the service with mock dependencies
            _service = new VatRegistrationService(_mockFactory.Object);
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldCallFranceStrategy_WhenCountryIsFrance()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.FR,
                CompanyName = "My Company",
                CompanyId = "12345"
            };

            var franceStrategyMock = new Mock<IVatRegistrationStrategy>();
            franceStrategyMock.Setup(x => x.RegisterVatAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            _mockFactory.Setup(f => f.GetStrategy(CountryCode.FR)).Returns(franceStrategyMock.Object);

            // Act
            await _service.RegisterVatAsync(request);

            // Assert
            franceStrategyMock.Verify(x => x.RegisterVatAsync(It.Is<VatRegistrationRequest>(r => r.Country == CountryCode.FR)), Times.Once);
            _mockQueueClient.Verify(q => q.EnqueueAsync("vat-registration-csv", It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldCallGermanyStrategy_WhenCountryIsGermany()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.DE,
                CompanyName = "My Company",
                CompanyId = "12345"
            };

            var germanyStrategyMock = new Mock<IVatRegistrationStrategy>();
            germanyStrategyMock.Setup(x => x.RegisterVatAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            _mockFactory.Setup(f => f.GetStrategy(CountryCode.DE)).Returns(germanyStrategyMock.Object);

            // Act
            await _service.RegisterVatAsync(request);

            // Assert
            germanyStrategyMock.Verify(x => x.RegisterVatAsync(It.Is<VatRegistrationRequest>(r => r.Country == CountryCode.DE)), Times.Once);

            // Verify that XML was generated for Germany strategy
            var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
            serializer.Serialize(stringWriter, request);
            var xml = stringWriter.ToString();
            _mockQueueClient.Verify(q => q.EnqueueAsync("vat-registration-xml", It.Is<string>(s => s == xml)), Times.Once);
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldCallUkStrategy_WhenCountryIsUk()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.GB,
                CompanyName = "My Company",
                CompanyId = "12345"
            };

            var ukStrategyMock = new Mock<IVatRegistrationStrategy>();
            ukStrategyMock.Setup(x => x.RegisterVatAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            _mockFactory.Setup(f => f.GetStrategy(CountryCode.GB)).Returns(ukStrategyMock.Object);

            // Act
            await _service.RegisterVatAsync(request);

            // Assert
            ukStrategyMock.Verify(x => x.RegisterVatAsync(It.Is<VatRegistrationRequest>(r => r.Country == CountryCode.GB)), Times.Once);

            _mockHttpClient.Verify(http => http.PostAsync("https://api.uktax.gov.uk", It.IsAny<VatRegistrationRequest>()), Times.Once);
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldThrowException_WhenStrategyIsNull()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.Unknown,
                CompanyName = "My Company",
                CompanyId = "12345"
            };

            _mockFactory.Setup(f => f.GetStrategy(CountryCode.Unknown)).Returns((IVatRegistrationStrategy) new UnsupportedCountryException(CountryCode.Unknown));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnsupportedCountryException>(() => _service.RegisterVatAsync(request));
            Assert.Equal("No VAT registration strategy found for the country: Unknown", exception.Message);
        }
    }

}
