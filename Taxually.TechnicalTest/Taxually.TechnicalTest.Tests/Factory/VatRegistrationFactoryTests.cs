using Xunit;
using FluentAssertions;
using Taxually.TechnicalTest.Infrastructure.Strategies;
using Taxually.TechnicalTest.Core.Models;
using Taxually.TechnicalTest.Infrastructure.Factory;
namespace Taxually.TechnicalTest.Tests.Unit.Factory
{
    public class VatRegistrationFactoryTests
    {
        private readonly VatRegistrationFactory _factory;

        public VatRegistrationFactoryTests()
        {
            _factory = new VatRegistrationFactory();
        }

        [Fact]
        public void GetStrategy_ShouldReturnUkStrategy_ForGB()
        {
            // Act
            var strategy = _factory.GetStrategy(CountryCode.GB);

            // Assert
            strategy.Should().BeOfType<UkVatRegistrationStrategy>();
        }

        [Fact]
        public void GetStrategy_ShouldThrowException_ForUnsupportedCountry()
        {
            // Act
            Action act = () => _factory.GetStrategy(CountryCode.Unknown);

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("Country not supported");
        }
    }

}
