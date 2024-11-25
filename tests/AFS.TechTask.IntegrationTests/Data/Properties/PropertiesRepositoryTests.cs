using AFS.TechTask.Application.Properties;
using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Data.Properties.Photos;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System.Data;
using static AFS.TechTask.IntegrationTests.Data.Properties.PropertyTestConstants;

namespace AFS.TechTask.IntegrationTests.Data.Properties
{
    public class PropertiesRepositoryTests
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        private readonly IPropertiesDataSource propertiesDataSource;
        private readonly IBedroomsDataSource bedroomsDataSource;
        private readonly IPhotosDataSource photosDataSource;

        private readonly Mock<IPropertiesDataSource> mockPropertiesDataSource;
        private readonly Mock<IBedroomsDataSource> mockBedroomsDataSource;
        private readonly Mock<IPhotosDataSource> mockPhotosDataSource;

        private static readonly IOptions<FeatureFlagsOptions> FeatureFlagsOptions = Options.Create(new FeatureFlagsOptions()
        {
            EnablePropertyIngest = true
        });

        public PropertiesRepositoryTests()
        {
            this.dbConnectionFactory = new DbConnectionFactory(Options.Create(new ConnectionStringOptions()));

            this.propertiesDataSource = new PropertiesDataSource(this.dbConnectionFactory);
            this.bedroomsDataSource = new BedroomsDataSource(this.dbConnectionFactory);
            this.photosDataSource = new PhotosDataSource(this.dbConnectionFactory);

            this.mockPropertiesDataSource = new Mock<IPropertiesDataSource>(MockBehavior.Strict);
            this.mockBedroomsDataSource = new Mock<IBedroomsDataSource>(MockBehavior.Strict);
            this.mockPhotosDataSource = new Mock<IPhotosDataSource>(MockBehavior.Strict);
        }

        [Fact]
        public async Task GetPropertyByIdAsync_FeatureFlagDisabled_ThrowsNotImplementedException()
        {
            // Arrange
            PropertyIngestResult expected = PropertyIngestResult.InvalidResult(DateTime.Now);
            PropertiesRepository sut = new PropertiesRepository(
                this.dbConnectionFactory,
                this.mockPropertiesDataSource.Object,
                this.mockBedroomsDataSource.Object,
                this.mockPhotosDataSource.Object,
                Options.Create(new FeatureFlagsOptions()
                {
                    EnablePropertyIngest = false
                }));

            // Act
            Func<Task> action = () => sut.GetPropertyByIdAsync(123);

            // Assert
            await action.Should().ThrowAsync<NotImplementedException>();
        }

        [Fact]
        public async Task InsertPropertyAsync_PropertiesThrows_RollsbackTransaction()
        {
            // Arrange
            Property input = PropertyFactory.Create(CreatePropertyDataModel(),
                BedroomDataModels(0, Studio.ExactNumberOfBedrooms),
                PhotoDataModels(0, Studio.MaxNumberOfPhotos));

            this.mockPropertiesDataSource.Setup(x => x.InsertPropertyAsync(It.IsAny<PropertyDataModel>(), It.IsAny<IDbTransaction>())).Throws<Exception>();

            PropertiesRepository sut = CreateRepository(this.mockPropertiesDataSource.Object, this.mockBedroomsDataSource.Object);

            // Act
            Func<Task> action = () => sut.InsertPropertyAsync(DateTime.MinValue, input);

            // Assert
            await action.Should().ThrowAsync<Exception>();
            this.mockBedroomsDataSource.Verify(x => x.InsertBedroomsAsync(It.IsAny<ICollection<BedroomDataModel>>(), It.IsAny<IDbTransaction>()), 
                Times.Never);
        }

        [Fact]
        public async Task InsertPropertyAsync_BedroomsThrows_RollsbackTransaction()
        {
            // Arrange
            Property input = PropertyFactory.Create(CreatePropertyDataModel(),
                BedroomDataModels(0, House.MaxNumberOfBedrooms),
                PhotoDataModels(0, Studio.MaxNumberOfPhotos));

            BedroomDataModel capture = null;
            this.mockBedroomsDataSource
                .Setup(x => x.InsertBedroomsAsync(It.IsAny<ICollection<BedroomDataModel>>(), It.IsAny<IDbTransaction>()))
                .Callback<ICollection<BedroomDataModel>, IDbTransaction>((bedrooms, _) => capture = bedrooms.First())
                .Throws<Exception>();

            PropertiesRepository sut = CreateRepository(
                bedrooms: this.mockBedroomsDataSource.Object, 
                photos: this.mockPhotosDataSource.Object);

            // Act
            Func<Task> action = () => sut.InsertPropertyAsync(DateTime.MinValue, input);

            // Assert
            await action.Should().ThrowAsync<Exception>();
            this.mockPhotosDataSource.Verify(x => x.InsertPhotosAsync(It.IsAny<ICollection<PhotoDataModel>>(), It.IsAny<IDbTransaction>()), 
                Times.Never);

            Func<Task> property = () => this.propertiesDataSource.GetPropertyByIdAsync(capture.PropertyId);
            await property.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task InsertPropertyAsync_PhotosThrows_RollsbackTransaction()
        {
            // Arrange
            Property input = PropertyFactory.Create(CreatePropertyDataModel(),
                BedroomDataModels(0, House.MaxNumberOfBedrooms),
                PhotoDataModels(0, House.MaxNumberOfPhotos));

            PhotoDataModel capture = null;
            this.mockPhotosDataSource
                .Setup(x => x.InsertPhotosAsync(It.IsAny<ICollection<PhotoDataModel>>(), It.IsAny<IDbTransaction>()))
                .Callback<ICollection<PhotoDataModel>, IDbTransaction>((photos, _) => capture = photos.First())
                .Throws<Exception>();

            PropertiesRepository sut = CreateRepository(photos: this.mockPhotosDataSource.Object);

            // Act
            Func<Task> action = () => sut.InsertPropertyAsync(DateTime.MinValue, input);

            // Assert
            await action.Should().ThrowAsync<Exception>();

            Func<Task> property = () => this.propertiesDataSource.GetPropertyByIdAsync(capture.PropertyId);
            await property.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task InsertAndGetPropertyByIdAsync_WithValidDataModels_ReturnsProperty()
        {
            // Arrange
            PropertyDataModel property = CreatePropertyDataModel(
                propertyType: (int)PropertyTypes.Studio,
                country: UK.Name);
            ICollection<BedroomDataModel> bedrooms = BedroomDataModels(0, Studio.ExactNumberOfBedrooms);
            ICollection<PhotoDataModel> photos = PhotoDataModels(0, Studio.MaxNumberOfPhotos);

            Property expected = PropertyFactory.Create(property, bedrooms, photos);

            PropertiesRepository sut = CreateRepository();

            int propertyId = await sut.InsertPropertyAsync(DateTime.MinValue, expected);

            // Act
            Property result = await sut.GetPropertyByIdAsync(propertyId);

            // Assert
            result.Should().BeEquivalentTo(expected, o => o
                .Excluding(p => p.Id)
                .Excluding(p => p.Bedrooms)
                .Excluding(p => p.Photos));
            result.Id.Should().Be(propertyId);

            result.Photos.Should().BeEquivalentTo(expected.Photos, o => o
                .Excluding(p => p.Id)
                .Excluding(p => p.PropertyId));
            result.Bedrooms.Should().BeEquivalentTo(expected.Bedrooms, o => o
                .Excluding(b => b.Id)
                .Excluding(b => b.PropertyId));

            result.Photos.All(p => p.PropertyId == propertyId).Should().BeTrue();
            result.Bedrooms.All(b => b.PropertyId == propertyId).Should().BeTrue();

            result.Photos.All(p => p.Id > 0).Should().BeTrue();
            result.Bedrooms.All(b => b.Id > 0).Should().BeTrue();
        }

        private PropertiesRepository CreateRepository(IPropertiesDataSource properties = null, IBedroomsDataSource bedrooms = null, IPhotosDataSource photos = null)
        {
            return new PropertiesRepository(
                this.dbConnectionFactory,
                properties ?? this.propertiesDataSource,
                bedrooms ?? this.bedroomsDataSource,
                photos ?? this.photosDataSource,
                FeatureFlagsOptions);
        }
    }
}
