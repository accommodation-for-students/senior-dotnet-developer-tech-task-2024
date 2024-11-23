using AFS.TechTask.Common;
using FluentAssertions;

namespace AFS.TechTask.UnitTests.Properties.Common
{
    public class CountryTests
    {
        [Theory]
        [InlineData(Country.UK, '£', "GBP")]
        [InlineData(Country.ROI, '€', "EUR")]
        public void NewCountry_ValidCountryName_ConstructsOK(string countryName, char symbol, string acronym)
        {
            // Act
            Country result = new Country(countryName);

            // Assert
            result.Name.Should().Be(countryName);
            result.CurrencySymbol.Should().Be(symbol);
            result.CurrencyAcronym.Should().Be(acronym);
        }

        [Theory]
        [InlineData("UK")]
        [InlineData("ROI")]
        [InlineData("FR")]
        [InlineData("Great Britain")]
        [InlineData("Ireland")]
        [InlineData("France")]
        public void NewCountry_UnsupportedCountryName_Throws(string countryName)
        {
            // Act
            Action action = () => new Country(countryName);

            // Assert
            action.Should().Throw<InvalidCountryException>();
        }
    }
}
