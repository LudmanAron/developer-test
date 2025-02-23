using Xunit;
using Moq;
using Taxually.TechnicalTest.Infrastructure.Services;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Core.Interfaces;
using Taxually.TechnicalTest.Core.Exceptions;

namespace Taxually.TechnicalTest.Tests.Unit.Services
{

    public class VatRegistrationServiceTests
    {
        private readonly Mock<IVatRegistrationFactory> _mockFactory;
        private readonly VatRegistrationService _service;

        public VatRegistrationServiceTests()
        {
            _mockFactory = new Mock<IVatRegistrationFactory>();
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
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldThrowException_WhenStrategyIsNotFound()
        {
            // Arrange
            var request = new VatRegistrationRequest
            {
                Country = CountryCode.Unknown,
                CompanyName = "My Company",
                CompanyId = "12345"
            };

            _mockFactory.Setup(f => f.GetStrategy(CountryCode.Unknown)).Throws(new UnsupportedCountryException(CountryCode.Unknown));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnsupportedCountryException>(() => _service.RegisterVatAsync(request));
            Assert.Equal($"VAT registration is not supported for country: {CountryCode.Unknown}", exception.Message);
        }
    }
}
