using AFS.TechTask.Application.Properties;
using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Common;
using AFS.TechTask.Domain.Properties;
using FluentAssertions;

using static AFS.TechTask.UnitTests.Constants.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Application.Properties
{
    public class PropertyFactoryTests
    {
        [Fact]
        public void Create_IngestResponse_ValidStudio_ReturnsStudio()
        {
            // Arrange
            ICollection<BedroomResponse> bedrooms = CreateBedroomResponses(Studio.ExactNumberOfBedrooms);
            ICollection<string> photos = CreatePhotoResponses(Studio.MaxNumberOfPhotos);

            Studio expected = new(
                bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)).ToArray(),
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
        public void Create_IngestResponse_ValidHouse_ReturnsHouse()
        {
            // Arrange
            ICollection<BedroomResponse> bedrooms = CreateBedroomResponses(House.MaxNumberOfBedrooms);
            ICollection<string> photos = CreatePhotoResponses(House.MaxNumberOfPhotos);

            House expected = new(
                bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)).ToArray(),
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
        public void Create_IngestResponse_ValidFlat_ReturnsFlat()
        {
            // Arrange
            ICollection<BedroomResponse> bedrooms = CreateBedroomResponses(Flat.MaxNumberOfBedrooms);
            ICollection<string> photos = CreatePhotoResponses(Flat.MaxNumberOfPhotos);

            Flat expected = new (
                bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)).ToArray(),
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
        public void Create_IngestResponse_InvalidPropertyType_Throws()
        {
            // Arrange
            PropertyResponse input = CreatePropertyResponse(propertyType: "Caravan");

            // Act
            Action action = () => PropertyFactory.Create(input);

            // Assert
            action.Should().Throw<InvalidPropertyTypeException>();
        }

        [Theory]
        [MemberData(nameof(NullIngestResponseTestData))]
        public void Create_IngestResponse_NullValues_Throws(PropertyResponse input, Type exceptionType)
        {
            // Act
            Action action = () => PropertyFactory.Create(input);

            // Assert
            action.Should().Throw<Exception>().Which.GetType().Should().Be(exceptionType);
        }

        public static IEnumerable<object[]> NullIngestResponseTestData()
        {
            yield return new object[]
            {
                null, typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                new PropertyResponse(
                    PropertyType: null,
                    Bedrooms: CreateBedroomResponses(2),
                    Photos: CreatePhotoResponses(Property.MaxNumberOfPhotos),
                    Country: UK.Name),
                typeof(InvalidPropertyTypeException)
            };

            yield return new object[]
            {
                new PropertyResponse(
                    PropertyType: PropertyTypes.House.ToString(),
                    Bedrooms: null,
                    Photos: CreatePhotoResponses(Property.MaxNumberOfPhotos),
                    Country: UK.Name),
                typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                new PropertyResponse(
                    PropertyType: PropertyTypes.House.ToString(),
                    Bedrooms: CreateBedroomResponses(5),
                    Photos: null,
                    Country: UK.Name),
                typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                new PropertyResponse(
                    PropertyType: PropertyTypes.House.ToString(),
                    Bedrooms: CreateBedroomResponses(5),
                    Photos: CreatePhotoResponses(Property.MaxNumberOfPhotos),
                    Country: null),
                typeof(InvalidCountryException)
            };
        }

        [Fact]
        public void Create_DataModels_ValidStudio_ReturnsStudio()
        {
            // Arrange
            PropertyDataModel property = CreatePropertyDataModel(
                propertyType: (int)PropertyTypes.Studio,
                country: UK.Name);
            ICollection<BedroomDataModel> bedrooms = BedroomDataModels(Studio.ExactNumberOfBedrooms);
            ICollection<PhotoDataModel> photos = PhotoDataModels(Studio.MaxNumberOfPhotos);

            Studio expected = new(
                bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)
                {
                    Id = b.BedroomId, PropertyId = b.PropertyId
                }).ToArray(),
                photos.Select(p => new Photo(p.Uri) {  Id = p.PhotoId, PropertyId = p.PropertyId }).ToArray(),
                UK)
            {
                Id = property.Id,
                LastUpdated = property.IngestRunId
            };

            // Act
            Property result = PropertyFactory.Create(property, bedrooms, photos);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Studio>();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_DataModels_ValidHouse_ReturnsHouse()
        {
            // Arrange
            PropertyDataModel property = CreatePropertyDataModel(
                propertyType: (int)PropertyTypes.House,
                country: UK.Name);
            ICollection<BedroomDataModel> bedrooms = BedroomDataModels(House.MaxNumberOfBedrooms);
            ICollection<PhotoDataModel> photos = PhotoDataModels(House.MaxNumberOfPhotos);

            House expected = new(
                bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)
                {
                    Id = b.BedroomId,
                    PropertyId = b.PropertyId
                }).ToArray(),
                photos.Select(p => new Photo(p.Uri) { Id = p.PhotoId, PropertyId = p.PropertyId }).ToArray(),
                UK)
            {
                Id = property.Id,
                LastUpdated = property.IngestRunId
            };

            // Act
            Property result = PropertyFactory.Create(property, bedrooms, photos);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<House>();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_DataModels_ValidFlat_ReturnsFlat()
        {
            // Arrange
            PropertyDataModel property = CreatePropertyDataModel(
                propertyType: (int)PropertyTypes.Flat,
                country: UK.Name);
            ICollection<BedroomDataModel> bedrooms = BedroomDataModels(Flat.MaxNumberOfBedrooms);
            ICollection<PhotoDataModel> photos = PhotoDataModels(Flat.MaxNumberOfPhotos);

            Flat expected = new(
                bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)
                {
                    Id = b.BedroomId,
                    PropertyId = b.PropertyId
                }).ToArray(),
                photos.Select(p => new Photo(p.Uri) { Id = p.PhotoId, PropertyId = p.PropertyId }).ToArray(),
                UK)
            {
                Id = property.Id,
                LastUpdated = property.IngestRunId
            };

            // Act
            Property result = PropertyFactory.Create(property, bedrooms, photos);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Flat>();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_DataModels_InvalidPropertyType_Throws()
        {
            // Arrange
            PropertyDataModel property = CreatePropertyDataModel(propertyType: int.MinValue);
            
            // Act
            Action action = () => PropertyFactory.Create(property, 
                BedroomDataModels(Flat.MaxNumberOfBedrooms), 
                PhotoDataModels(Flat.MaxNumberOfPhotos));

            // Assert
            action.Should().Throw<InvalidPropertyTypeException>();
        }

        [Theory]
        [MemberData(nameof(NullDataModelTestData))]
        public void Create_DataModel_NullValues_Throws(PropertyDataModel property, ICollection<BedroomDataModel> bedrooms,
            ICollection<PhotoDataModel> photos, Type exceptionType)
        {
            // Act
            Action action = () => PropertyFactory.Create(property, bedrooms, photos);

            // Assert
            action.Should().Throw<Exception>().Which.GetType().Should().Be(exceptionType);
        }

        public static IEnumerable<object[]> NullDataModelTestData()
        {
            yield return new object[]
            {
                null, BedroomDataModels(Flat.MaxNumberOfBedrooms), PhotoDataModels(Flat.MaxNumberOfPhotos), 
                typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                CreatePropertyDataModel(), null, PhotoDataModels(Flat.MaxNumberOfPhotos),
                typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                CreatePropertyDataModel(), BedroomDataModels(House.MaxNumberOfBedrooms), null,
                typeof(ArgumentNullException)
            };
        }
    }
}
