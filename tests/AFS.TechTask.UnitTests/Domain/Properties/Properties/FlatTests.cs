using AFS.TechTask.Domain.Common;
using AFS.TechTask.Domain.Properties;
using FluentAssertions;

using static AFS.TechTask.UnitTests.Constants.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Domain.Properties
{
    public class FlatTests
    {
        [Fact]
        public void Verify_FlatConstants_TestAssumptions()
        {
            // Assert
            Flat.MinNumberOfBedrooms.Should().Be(1);
            Flat.MaxNumberOfBedrooms.Should().Be(14);
            Flat.MaxNumberOfPhotos.Should().Be(14);
        }

        [Theory]
        [InlineData(Flat.MinNumberOfBedrooms, 0)]
        [InlineData(Flat.MinNumberOfBedrooms, 1)]
        [InlineData(Flat.MaxNumberOfBedrooms, Flat.MaxNumberOfPhotos)]
        public void NewFlat_ValidParams_ConstructsOK(int numOfRooms, int numOfPhotos)
        {
            // Arrange
            var rooms = LargeDoubleBedrooms(numOfRooms);
            var photos = HousePhotos(numOfPhotos);
            var country = UK;

            // Act
            Flat result = new Flat(rooms, photos, country);

            // Assert
            result.Bedrooms.Should().BeEquivalentTo(rooms);
            result.Photos.Should().BeEquivalentTo(photos);
            result.Country.Should().BeEquivalentTo(country);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void NewFlat_NullArguments_Throws(bool roomsAreNull, bool photosAreNull, bool countryIsNull)
        {
            // Arrange
            IReadOnlyCollection<Bedroom> bedrooms = roomsAreNull ? null : LargeDoubleBedrooms(5);
            IReadOnlyCollection<Photo> photos = photosAreNull ? null : HousePhotos(5);
            Country country = countryIsNull ? null : UK;

            // Act
            Action action = () => new Flat(bedrooms, photos, country);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(Flat.MaxNumberOfBedrooms + 1)]
        public void NewFlat_InvalidNumberOfRooms_Throws(int numOfRooms)
        {
            // Arrange
            IReadOnlyCollection<Bedroom> bedrooms = LargeDoubleBedrooms(numOfRooms);

            // Act
            Action action = () => CreateFlat(bedrooms);

            // Assert
            action.Should().Throw<InvalidNumberOfBedroomsException>();
        }

        [Theory]
        [InlineData(Flat.MaxNumberOfPhotos + 1)]
        [InlineData(100)]
        public void NewFlat_InvalidNumberOfPhotos_Throws(int numOfPhotos)
        {
            // Arrange
            IReadOnlyCollection<Photo> photos = HousePhotos(numOfPhotos);

            // Act
            Action action = () => CreateFlat(photos: photos);

            // Assert
            action.Should().Throw<InvalidNumberOfPhotosException>();
        }

        [Theory]
        [MemberData(nameof(PriceFromMemberData))]
        public void Studio_PriceFrom_CalculatesMinimumRent(Bedroom[] bedrooms, uint? expected)
        {
            // Arrange
            Flat studio = CreateFlat(bedrooms);

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

        private Flat CreateFlat(IReadOnlyCollection<Bedroom> bedrooms = null, IReadOnlyCollection<Photo> photos = null)
            => new Flat(
                bedrooms ?? LargeDoubleBedrooms(5), 
                photos ?? HousePhotos(10), 
                country: UK);
    }
}
