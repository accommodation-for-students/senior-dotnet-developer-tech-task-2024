using AFS.TechTask.Domain.Properties;
using FluentAssertions;

namespace AFS.TechTask.UnitTests.Domain.Properties
{
    public class PhotoTests
    {
        [Theory]
        [InlineData("http://happy.com/123")]
        [InlineData("http://happy.com/go/lucky/123")]
        [InlineData("http://happy.com?sad=false")]
        public void NewPhoto_WithValidUri_ConstructsOK(string uri)
        {
            // Act
            Photo result = new(uri);

            // Assert
            result.Uri.Should().Be(uri);
        }

        [Fact]
        public void Photo_ToString_Should_ReturnUri()
        {
            // Arrange
            string uri = "http://happy.com";

            // Act
            Photo result = new(uri);

            // Assert
            result.ToString().Should().Be(uri);
        }

        [Theory]
        [InlineData("")]
        [InlineData(".com")]
        [InlineData("happy.com")]
        [InlineData("http:/happy.com")]
        public void NewPhoto_WithInvalidUri_Throws(string uri)
        {
            // Act
            Action action = () => new Photo(uri);

            // Assert
            action.Should().Throw<UriFormatException>();
        }
    }
}
