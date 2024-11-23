using AFS.TechTask.Properties;
using AFS.TechTask.Properties.Ingest;
using AFS.TechTask.Properties.Ingest.Models;
using AFS.TechTask.Properties.Properties;
using FluentAssertions;

using static AFS.TechTask.UnitTests.Properties.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Properties.Ingest
{
    public class PropertyFactoryTests
    {
        [Fact]
        public void Create_ValidStudio_ReturnsStudio()
        {
            // Arrange
            ICollection<BedroomResponse> bedrooms = CreateBedroomResponses(Studio.ExactNumberOfBedrooms);
            ICollection<string> photos = CreatePhotoResponses(Studio.MaxNumberOfPhotos);

            Studio expected = new(
                bedrooms.Select(b => new Bedroom(b)).ToArray(),
                photos.Select(p => new Photo(p)).ToArray(),
                UK);

            PropertyResponse input = CreatePropertyResponse(
                propertyType: PropertyTypes.Studio.ToString(),
                bedrooms, photos, country: UK.Name);

            // Act
            Property result = PropertyFactory.Create(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Studio>();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_ValidHouse_ReturnsHouse()
        {
            // Arrange
            ICollection<BedroomResponse> bedrooms = CreateBedroomResponses(House.MaxNumberOfBedrooms);
            ICollection<string> photos = CreatePhotoResponses(House.MaxNumberOfPhotos);

            House expected = new(
                bedrooms.Select(b => new Bedroom(b)).ToArray(),
                photos.Select(p => new Photo(p)).ToArray(),
                ROI);

            PropertyResponse input = CreatePropertyResponse(
                propertyType: PropertyTypes.House.ToString(),
                bedrooms, photos, country: ROI.Name);

            // Act
            Property result = PropertyFactory.Create(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<House>();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_ValidFlat_ReturnsFlat()
        {
            // Arrange
            ICollection<BedroomResponse> bedrooms = CreateBedroomResponses(Flat.MaxNumberOfBedrooms);
            ICollection<string> photos = CreatePhotoResponses(Flat.MaxNumberOfPhotos);

            Flat expected = new (
                bedrooms.Select(b => new Bedroom(b)).ToArray(),
                photos.Select(p => new Photo(p)).ToArray(),
                UK);

            PropertyResponse input = CreatePropertyResponse(
                propertyType: PropertyTypes.Flat.ToString(),
                bedrooms, photos, country: UK.Name);

            // Act
            Property result = PropertyFactory.Create(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Flat>();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_InvalidPropertyType_Throws()
        {
            // Arrange
            PropertyResponse input = CreatePropertyResponse(propertyType: "Caravan");

            // Act
            Action action = () => PropertyFactory.Create(input);

            // Assert
            action.Should().Throw<InvalidPropertyTypeException>();
        }
    }
}
