using AFS.TechTask.Application.Properties;
using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

using static AFS.TechTask.UnitTests.Constants.PropertyTestConstants;

namespace AFS.TechTask.UnitTests.Application.Properties
{
    public class PropertyServiceTests
    {
        private readonly PropertyService service;

        private readonly Mock<IPropertyIngestService> mockPropertyIngestService;
        private readonly Mock<IPropertiesRepository> mockPropertiesRepository;
        private static readonly IOptions<FeatureFlagsOptions> FeatureFlags = Options.Create(new FeatureFlagsOptions()
        {
            EnablePropertyIngest = true
        });

        public PropertyServiceTests() 
        { 
            this.mockPropertyIngestService = new Mock<IPropertyIngestService>(MockBehavior.Strict);
            this.mockPropertiesRepository = new Mock<IPropertiesRepository>(MockBehavior.Strict);

            this.service = new PropertyService(this.mockPropertyIngestService.Object, this.mockPropertiesRepository.Object, FeatureFlags);
        }

        [Fact]
        public async Task RunIngestPropertiesJobAsync_FeatureFlagDisabled_ReturnsEarly()
        {
            // Arrange
            PropertyService sut = new (this.mockPropertyIngestService.Object, this.mockPropertiesRepository.Object, Options.Create(new FeatureFlagsOptions()
            {
                EnablePropertyIngest = false
            }));

            // Act
            await sut.RunIngestPropertiesJobAsync();

            // Assert
            mockPropertyIngestService.Verify(x => x.IngestPropertiesAsync(), Times.Never);
            mockPropertiesRepository.Verify(x => x.InsertPropertyAsync(It.IsAny<DateTime>(), It.IsAny<Property>()), Times.Never);
        }

        [Fact]
        public async Task RunIngestPropertiesJobAsync_IngestFails_DoesNotTryToPersistProperties()
        {
            // Arrange
            mockPropertyIngestService.Setup(x => x.IngestPropertiesAsync())
                .ReturnsAsync(new PropertyIngestResult(Run: DateTime.Now, Success: false, ValidProperties: [], InvalidProperties: []));

            // Act
            Func<Task> action = () => this.service.RunIngestPropertiesJobAsync();

            // Assert
            await action.Should().NotThrowAsync();
            mockPropertyIngestService.Verify(x => x.IngestPropertiesAsync(), Times.Once);
            mockPropertiesRepository.Verify(x => x.InsertPropertyAsync(It.IsAny<DateTime>(), It.IsAny<Property>()), Times.Never);
        }

        [Fact]
        public async Task RunIngestPropertiesJobAsync_IngestSucceeds_PersistsProperties()
        {
            // Arrange
            Property property1 = CreateProperty();
            Property property2 = CreateProperty();
            Property property3 = CreateProperty();
            PropertyIngestResult result = new (Run: DateTime.Now, Success: true, ValidProperties: [property1, property2, property3], InvalidProperties: []);

            mockPropertyIngestService.Setup(x => x.IngestPropertiesAsync())
                .ReturnsAsync(result);

            // Act
            Func<Task> action = () => this.service.RunIngestPropertiesJobAsync();

            // Assert
            await action.Should().NotThrowAsync();
            mockPropertyIngestService.Verify(x => x.IngestPropertiesAsync(), Times.Once);
            mockPropertiesRepository.Verify(x => x.InsertPropertyAsync(result.Run, property1), Times.Once);
            mockPropertiesRepository.Verify(x => x.InsertPropertyAsync(result.Run, property2), Times.Once);
            mockPropertiesRepository.Verify(x => x.InsertPropertyAsync(result.Run, property3), Times.Once);
        }
    }
}
