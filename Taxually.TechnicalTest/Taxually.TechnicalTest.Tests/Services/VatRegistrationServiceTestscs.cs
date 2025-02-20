using Xunit;
using Moq;
using FluentAssertions;
using Taxually.TechnicalTest.Infrastructure.Services;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Infrastructure.Strategies;
using Taxually.TechnicalTest.Infrastructure.Factory;

namespace Taxually.TechnicalTest.Tests.Unit.Services
{

    public class VatRegistrationServiceTests
    {
        private readonly VatRegistrationService _service;
        private readonly Mock<VatRegistrationFactory> _factoryMock;

        public VatRegistrationServiceTests()
        {
            _factoryMock = new Mock<VatRegistrationFactory>();
            _service = new VatRegistrationService(_factoryMock.Object);
        }

        [Fact]
        public void RegisterVat_ShouldCallCorrectFactoryMethod()
        {
            // Arrange
            var request = new VatRegistrationRequest { Country = CountryCode.GB, CompanyName = "TestCo", CompanyId = "123" };
            _factoryMock.Setup(f => f.GetStrategy(CountryCode.GB)).Returns(new UkVatRegistrationStrategy());

            // Act
            var result = _service.RegisterVatAsync(request);

            // Assert
            _factoryMock.Verify(f => f.GetStrategy(CountryCode.GB), Times.Once);
            result.Should().BeTrue();
        }

        [Fact]
        public void RegisterVat_ShouldThrowException_ForUnsupportedCountry()
        {
            // Arrange
            var request = new VatRegistrationRequest { Country = CountryCode.GB, CompanyName = "TestCo", CompanyId = "123" };

            _factoryMock.Setup(f => f.GetStrategy(CountryCode.GB)).Throws(new NotSupportedException("Country not supported"));

            // Act
            Action act = () => _service.RegisterVatAsync(request);

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("Country not supported");
        }
    }

}
