using AFS.TechTask.Common;
using AFS.TechTask.Properties;
using AFS.TechTask.Properties.Property;
using FluentAssertions;
using static AFS.TechTask.UnitTests.Properties.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Properties.Property
{
    public class StudioTests
    {
        [Theory]
        [InlineData(Studio.ExactNumberOfBedrooms, 0)]
        [InlineData(Studio.ExactNumberOfBedrooms, 1)]
        [InlineData(Studio.ExactNumberOfBedrooms, Studio.MaxNumberOfPhotos)]
        public void NewStudio_ValidParams_ConstructsOK(int numOfRooms, int numOfPhotos)
        {
            // Arrange
            var rooms = LargeDoubleBedrooms(numOfRooms);
            var photos = HousePhotos(numOfPhotos);
            var country = UK;

            // Act
            Studio result = new Studio(rooms, photos, country);

            // Assert
            result.Bedrooms.Should().BeEquivalentTo(rooms);
            result.Photos.Should().BeEquivalentTo(photos);
            result.Country.Should().BeEquivalentTo(country);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void NewStudio_NullArguments_Throws(bool roomsAreNull, bool photosAreNull, bool countryIsNull)
        {
            // Arrange
            IReadOnlyCollection<Bedroom> bedrooms = roomsAreNull ? null : LargeDoubleBedrooms(1);
            IReadOnlyCollection<Photo> photos = photosAreNull ? null : HousePhotos(5);
            Country country = countryIsNull ? null : UK;

            // Act
            Action action = () => new Studio(bedrooms, photos, country);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(Studio.MaxNumberOfPhotos + 1)]
        public void NewStudio_InvalidNumberOfRooms_Throws(int numOfRooms) 
        {
            // Arrange
            IReadOnlyCollection<Bedroom> bedrooms = LargeDoubleBedrooms(numOfRooms);

            // Act
            Action action = () => CreateStudio(bedrooms);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(Studio.MaxNumberOfPhotos + 1)]
        [InlineData(100)]
        public void NewStudio_InvalidNumberOfPhotos_Throws(int numOfPhotos)
        {
            // Arrange
            IReadOnlyCollection<Photo> photos = HousePhotos(numOfPhotos);

            // Act
            Action action = () => CreateStudio(photos: photos);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(PriceFromMemberData))]
        public void Studio_PriceFrom_CalculatesMinimumRent(Bedroom bedroom, uint? expected)
        {
            // Arrange
            Studio studio = CreateStudio([bedroom]);

            // Act
            uint? result = studio.PriceFrom();

            // Assert
            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> PriceFromMemberData()
        {
            yield return new object[] { new Bedroom(true, LargeRoom, DoubleBed, 100, 250), 100u };
            yield return new object[] { new Bedroom(false, LargeRoom, DoubleBed, 100, 250), default(uint?) };
        }

        private Studio CreateStudio(IReadOnlyCollection<Bedroom> bedrooms = null, IReadOnlyCollection<Photo> photos = null)
            => new Studio(
                bedrooms ?? LargeDoubleBedrooms(1), 
                photos ?? HousePhotos(10), 
                country: UK);
    }
}
