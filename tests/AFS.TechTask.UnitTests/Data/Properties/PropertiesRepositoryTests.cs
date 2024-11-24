using AFS.TechTask.Application.Properties;
using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

using static AFS.TechTask.UnitTests.Constants.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Data.Properties
{
    public class PropertiesRepositoryTests
    {
        private readonly PropertiesRepository repository;

        private readonly Mock<IPropertiesDataSource> mockPropertiesDataSource;
        private readonly Mock<IBedroomsDataSource> mockBedroomsDataSource;
        private readonly Mock<IPhotosDataSource> mockPhotosDataSource;

        private static readonly IOptions<FeatureFlagsOptions> FeatureFlagsOptions = Options.Create(new FeatureFlagsOptions()
        {
            EnablePropertyIngest = true
        });

        public PropertiesRepositoryTests()
        {
            this.mockPropertiesDataSource = new Mock<IPropertiesDataSource>(MockBehavior.Strict);
            this.mockBedroomsDataSource = new Mock<IBedroomsDataSource>(MockBehavior.Strict);
            this.mockPhotosDataSource = new Mock<IPhotosDataSource>(MockBehavior.Strict);

            this.repository = new PropertiesRepository(
                this.mockPropertiesDataSource.Object,
                this.mockBedroomsDataSource.Object,
                this.mockPhotosDataSource.Object,
                FeatureFlagsOptions);
        }

        [Fact]
        public async Task GetPropertyByIdAsync_FeatureFlagDisabled_ThrowsNotImplementedException()
        {
            // Arrange
            PropertyIngestResult expected = PropertyIngestResult.InvalidResult(DateTime.Now);
            PropertiesRepository sut = new PropertiesRepository(this.mockPropertiesDataSource.Object,
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
        public async Task InsertPropertyAsync_WithValidDataModels_ReturnsPropertyId()
        {
            // Arrange
            Property input = PropertyFactory.Create(CreatePropertyDataModel(),
                BedroomDataModels(Studio.ExactNumberOfBedrooms),
                PhotoDataModels(Studio.MaxNumberOfPhotos));

            this.mockPropertiesDataSource.Setup(x => x.InsertPropertyAsync(It.IsAny<PropertyDataModel>())).ReturnsAsync(PropertyId);
            this.mockBedroomsDataSource.Setup(x => x.InsertBedroomsAsync(It.IsAny<ICollection<BedroomDataModel>>())).Returns(Task.CompletedTask);
            this.mockPhotosDataSource.Setup(x => x.InsertPhotosAsync(It.IsAny<ICollection<PhotoDataModel>>())).Returns(Task.CompletedTask);

            // Act
            int result = await this.repository.InsertPropertyAsync(DateTime.MinValue, input);

            // Assert
            result.Should().Be(PropertyId);
        }

        [Fact]
        public async Task GetPropertyByIdAsync_WithValidDataModels_ReturnsResult()
        {
            // Arrange
            PropertyDataModel property = CreatePropertyDataModel(
                propertyType: (int)PropertyTypes.Studio,
                country: UK.Name);
            ICollection<BedroomDataModel> bedrooms = BedroomDataModels(Studio.ExactNumberOfBedrooms);
            ICollection<PhotoDataModel> photos = PhotoDataModels(Studio.MaxNumberOfPhotos);

            this.mockPropertiesDataSource.Setup(x => x.GetPropertyByIdAsync(PropertyId)).ReturnsAsync(property);
            this.mockBedroomsDataSource.Setup(x => x.GetBedroomsByPropertyIdAsync(PropertyId)).ReturnsAsync(bedrooms);
            this.mockPhotosDataSource.Setup(x => x.GetPhotosByPropertyIdAsync(PropertyId)).ReturnsAsync(photos);

            Property expected = PropertyFactory.Create(property, bedrooms, photos);

            // Act
            Property result = await this.repository.GetPropertyByIdAsync(PropertyId);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
