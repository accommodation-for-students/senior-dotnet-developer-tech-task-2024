using AFS.TechTask.Common;
using AFS.TechTask.Properties;
using AFS.TechTask.Properties.Ingest;
using AFS.TechTask.Properties.Ingest.Models;
using AFS.TechTask.Properties.Properties;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

using static AFS.TechTask.UnitTests.Properties.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Properties.Ingest
{
    public class PropertyIngestServiceTests
    {
        private readonly PropertyIngestService service;

        private readonly Mock<IPropertyIngestClient> mockClient;

        private static readonly IOptions<FeatureFlagsOptions> FeatureFlagsOptions = Options.Create(new FeatureFlagsOptions()
        {
            EnablePropertyIngest = true
        });

        public PropertyIngestServiceTests()
        {
            this.mockClient = new Mock<IPropertyIngestClient>(MockBehavior.Strict);
            this.service = new PropertyIngestService(this.mockClient.Object, FeatureFlagsOptions);
        }

        [Fact]
        public async Task IngestPropertiesAsync_FeatureFlagDisabled_ReturnsEmptyResult()
        {
            // Arrange
            PropertyIngestResult expected = PropertyIngestResult.InvalidResult(DateTime.Now);
            PropertyIngestService sut = new PropertyIngestService(this.mockClient.Object, Options.Create(new FeatureFlagsOptions()
            {
                EnablePropertyIngest = false
            }));

            // Act
            PropertyIngestResult result = await sut.IngestPropertiesAsync();

            // Assert
            result.Should().BeEquivalentTo(expected, o => o.Excluding(r => r.Run));
        }

        [Fact]
        public async Task IngestPropertiesAsync_ClientThrowsException_ReturnsEmptyResult()
        {
            // Arrange
            PropertyIngestResult expected = PropertyIngestResult.InvalidResult(DateTime.Now);
            this.mockClient.Setup(x => x.GetPropertiesAsync()).Throws<Exception>();

            // Act
            PropertyIngestResult result = await this.service.IngestPropertiesAsync();

            // Assert
            result.Should().BeEquivalentTo(expected, o => o.Excluding(r => r.Run));
        }

        [Fact]
        public async Task IngestPropertiesAsync_AllInvalidProperties_ReturnsUnsuccessfulResult()
        {
            // Arrange
            IReadOnlyCollection<PropertyResponse> caravans = Enumerable.Range(0, 3)
                .Select(_ => new PropertyResponse(PropertyType: "caravan", Bedrooms: [], Photos: [], Country: "USA"))
                .ToArray();

            this.mockClient.Setup(x => x.GetPropertiesAsync()).ReturnsAsync(caravans);

            // Act
            PropertyIngestResult result = await this.service.IngestPropertiesAsync();

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task IngestPropertiesAsync_WithAllInvalidProperties_ReturnsResult()
        {
            // Arrange
            PropertyResponse[] properties =
            [
                CreatePropertyResponse(propertyType: PropertyTypes.Studio.ToString(), bedrooms: CreateBedroomResponses(2)),
                CreatePropertyResponse(propertyType: PropertyTypes.Flat.ToString(), photos: CreatePhotoResponses(25)),
                CreatePropertyResponse(propertyType: PropertyTypes.House.ToString(), bedrooms: [CreateBedroomResponse(roomSize: "not recognised")]),
            ];

            this.mockClient.Setup(x => x.GetPropertiesAsync()).ReturnsAsync(properties);

            PropertyIngestResult expected = new PropertyIngestResult(Run: DateTime.Now, Success: false, ValidProperties: [], 
                InvalidProperties:
                [
                    new InvalidPropertyIngest(properties[0], new InvalidNumberOfBedroomsException(PropertyTypes.Studio, Studio.ExactNumberOfBedrooms, 2)),
                    new InvalidPropertyIngest(properties[1], new InvalidNumberOfPhotosException(Property.MaxNumberOfPhotos, 25)),
                    new InvalidPropertyIngest(properties[2], new InvalidRoomSizeException("not recognised"))
                ]);

            // Act
            PropertyIngestResult result = await this.service.IngestPropertiesAsync();

            // Assert
            result.Should().BeEquivalentTo(expected, o => o.Excluding(r => r.Run));
        }

        [Fact]
        public async Task IngestPropertiesAsync_WithValidAndInvalidProperties_ReturnsResult()
        {
            // Arrange
            PropertyResponse[] properties =
            [
                CreatePropertyResponse(propertyType: PropertyTypes.Studio.ToString(), CreateBedroomResponses(1)),
                CreatePropertyResponse(propertyType: PropertyTypes.Flat.ToString()),
                CreatePropertyResponse(propertyType: PropertyTypes.House.ToString()),
                CreatePropertyResponse(propertyType: PropertyTypes.Studio.ToString(), bedrooms: CreateBedroomResponses(2)),
                CreatePropertyResponse(propertyType: PropertyTypes.Flat.ToString(), photos: CreatePhotoResponses(25)),
                CreatePropertyResponse(propertyType: PropertyTypes.House.ToString(), bedrooms: [CreateBedroomResponse(roomSize: "not recognised")]),
            ];

            this.mockClient.Setup(x => x.GetPropertiesAsync()).ReturnsAsync(properties);

            PropertyIngestResult expected = new PropertyIngestResult(Run: DateTime.Now, Success: true, 
                ValidProperties: 
                [
                    PropertyFactory.Create(properties[0]),
                    PropertyFactory.Create(properties[1]),
                    PropertyFactory.Create(properties[2])
                ], 
                InvalidProperties:
                [
                    new InvalidPropertyIngest(properties[3], new InvalidNumberOfBedroomsException(PropertyTypes.Studio, Studio.ExactNumberOfBedrooms, 2)),
                    new InvalidPropertyIngest(properties[4], new InvalidNumberOfPhotosException(Property.MaxNumberOfPhotos, 25)),
                    new InvalidPropertyIngest(properties[5], new InvalidRoomSizeException("not recognised"))
                ]);

            // Act
            PropertyIngestResult result = await this.service.IngestPropertiesAsync();

            // Assert
            result.Should().BeEquivalentTo(expected, o => o.Excluding(r => r.Run));
        }

        [Fact]
        public async Task IngestPropertiesAsync_HandlesNullProperties_ReturnsResult()
        {
            // Arrange
            PropertyResponse[] properties =
            [
                null,
                new PropertyResponse(
                    PropertyType: null,
                    Bedrooms: CreateBedroomResponses(2),
                    Photos: CreatePhotoResponses(Property.MaxNumberOfPhotos),
                    Country: UK.Name),
                new PropertyResponse(
                    PropertyType: PropertyTypes.House.ToString(),
                    Bedrooms: null,
                    Photos: CreatePhotoResponses(Property.MaxNumberOfPhotos),
                    Country: UK.Name),
                new PropertyResponse(
                    PropertyType: PropertyTypes.House.ToString(),
                    Bedrooms: CreateBedroomResponses(5),
                    Photos: null,
                    Country: UK.Name),
                new PropertyResponse(
                    PropertyType: PropertyTypes.House.ToString(),
                    Bedrooms: CreateBedroomResponses(5),
                    Photos: CreatePhotoResponses(Property.MaxNumberOfPhotos),
                    Country: null),
            ];

            this.mockClient.Setup(x => x.GetPropertiesAsync()).ReturnsAsync(properties);

            PropertyIngestResult expected = new PropertyIngestResult(Run: DateTime.Now, Success: false, ValidProperties: [],
                InvalidProperties:
                [
                    new InvalidPropertyIngest(null, new ArgumentNullException(nameof(PropertyResponse))),
                    new InvalidPropertyIngest(properties[1], new InvalidPropertyTypeException(null)),
                    new InvalidPropertyIngest(properties[2], new ArgumentNullException(nameof(Property.Bedrooms))),
                    new InvalidPropertyIngest(properties[3], new ArgumentNullException(nameof(Property.Photos))),
                    new InvalidPropertyIngest(properties[4], new InvalidCountryException(null))
                ]);

            // Act
            PropertyIngestResult result = await this.service.IngestPropertiesAsync();

            // Assert
            result.Should().BeEquivalentTo(expected, o => o.Excluding(r => r.Run));
        }

        [Fact]
        public async Task IngestPropertiesAsync_WithAllValidProperties_ReturnsResult()
        {
            // Arrange
            PropertyResponse[] properties =
            [
                CreatePropertyResponse(propertyType: PropertyTypes.Studio.ToString(), CreateBedroomResponses(1)),
                CreatePropertyResponse(propertyType: PropertyTypes.Flat.ToString()),
                CreatePropertyResponse(propertyType: PropertyTypes.House.ToString())
            ];

            this.mockClient.Setup(x => x.GetPropertiesAsync()).ReturnsAsync(properties);

            PropertyIngestResult expected = new PropertyIngestResult(Run: DateTime.Now, Success: true,
                ValidProperties:
                [
                    PropertyFactory.Create(properties[0]),
                    PropertyFactory.Create(properties[1]),
                    PropertyFactory.Create(properties[2])
                ],
                InvalidProperties: []);

            // Act
            PropertyIngestResult result = await this.service.IngestPropertiesAsync();

            // Assert
            result.Should().BeEquivalentTo(expected, o => o.Excluding(r => r.Run));
        }
    }
}
