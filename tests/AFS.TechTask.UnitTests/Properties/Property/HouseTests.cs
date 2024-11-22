using AFS.TechTask.Common;
using AFS.TechTask.Properties;
using AFS.TechTask.Properties.Property;
using FluentAssertions;

using static AFS.TechTask.UnitTests.Properties.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Properties.Property
{
    public class HouseTests
    {
        [Fact]
        public void Verify_HouseConstants_TestAssumptions()
        {
            // Assert
            House.MinNumberOfBedrooms.Should().Be(1);
            House.MaxNumberOfBedrooms.Should().Be(14);
            House.MaxNumberOfPhotos.Should().Be(14);
        }

        [Theory]
        [InlineData(House.MinNumberOfBedrooms, 0)]
        [InlineData(House.MinNumberOfBedrooms, 1)]
        [InlineData(House.MaxNumberOfBedrooms, House.MaxNumberOfPhotos)]
        public void NewHouse_ValidParams_ConstructsOK(int numOfRooms, int numOfPhotos)
        {
            // Arrange
            var rooms = LargeDoubleBedrooms(numOfRooms);
            var photos = HousePhotos(numOfPhotos);
            var country = UK;

            // Act
            House result = new House(rooms, photos, country);

            // Assert
            result.Bedrooms.Should().BeEquivalentTo(rooms);
            result.Photos.Should().BeEquivalentTo(photos);
            result.Country.Should().BeEquivalentTo(country);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void NewHouse_NullArguments_Throws(bool roomsAreNull, bool photosAreNull, bool countryIsNull)
        {
            // Arrange
            IReadOnlyCollection<Bedroom> bedrooms = roomsAreNull ? null : LargeDoubleBedrooms(5);
            IReadOnlyCollection<Photo> photos = photosAreNull ? null : HousePhotos(5);
            Country country = countryIsNull ? null : UK;

            // Act
            Action action = () => new House(bedrooms, photos, country);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(House.MaxNumberOfBedrooms + 1)]
        public void NewHouse_InvalidNumberOfRooms_Throws(int numOfRooms)
        {
            // Arrange
            IReadOnlyCollection<Bedroom> bedrooms = LargeDoubleBedrooms(numOfRooms);

            // Act
            Action action = () => CreateHouse(bedrooms);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(House.MaxNumberOfPhotos + 1)]
        [InlineData(100)]
        public void NewHouse_InvalidNumberOfPhotos_Throws(int numOfPhotos)
        {
            // Arrange
            IReadOnlyCollection<Photo> photos = HousePhotos(numOfPhotos);

            // Act
            Action action = () => CreateHouse(photos: photos);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(PriceFromMemberData))]
        public void Studio_PriceFrom_CalculatesMinimumRent(Bedroom[] bedrooms, uint? expected)
        {
            // Arrange
            House studio = CreateHouse(bedrooms);

            // Act
            uint? result = studio.PriceFrom();

            // Assert
            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> PriceFromMemberData() 
        {
            yield return new object[]
            {
                new Bedroom[] 
                { 
                    new Bedroom(true, LargeRoom, DoubleBed, 300, 250), 
                    new Bedroom(true, LargeRoom, DoubleBed, 100, 250),
                    new Bedroom(true, LargeRoom, DoubleBed, 200, 250)
                }, 
                100u
            };

            yield return new object[]
{
                new Bedroom[]
                {
                    new Bedroom(true, LargeRoom, DoubleBed, 300, 250),
                    new Bedroom(false, LargeRoom, DoubleBed, 100, 250),
                    new Bedroom(true, LargeRoom, DoubleBed, 200, 250)
                }, 200u
            };

            yield return new object[]
            {
                new Bedroom[]
                {
                    new Bedroom(false, LargeRoom, DoubleBed, 300, 250),
                    new Bedroom(false, LargeRoom, DoubleBed, 100, 250),
                    new Bedroom(false, LargeRoom, DoubleBed, 200, 250)
                }, default(uint?)
            };
        }

        private House CreateHouse(IReadOnlyCollection<Bedroom> bedrooms = null, IReadOnlyCollection<Photo> photos = null)
            => new House(
                bedrooms ?? LargeDoubleBedrooms(5), 
                photos ?? HousePhotos(10), 
                country: UK);
    }
}
