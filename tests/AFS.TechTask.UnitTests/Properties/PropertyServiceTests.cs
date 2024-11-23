using AFS.TechTask.Common;
using AFS.TechTask.Properties;
using AFS.TechTask.Properties.Ingest;
using AFS.TechTask.Properties.Ingest.Models;
using AFS.TechTask.Properties.Properties;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace AFS.TechTask.UnitTests.Properties
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
            mockPropertiesRepository.Verify(x => x.UpsertPropertiesAsync(It.IsAny<DateTime>(), It.IsAny<IReadOnlyCollection<Property>>()), Times.Never);
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
            mockPropertiesRepository.Verify(x => x.UpsertPropertiesAsync(It.IsAny<DateTime>(), It.IsAny<IReadOnlyCollection<Property>>()), Times.Never);
        }

        [Fact]
        public async Task RunIngestPropertiesJobAsync_IngestSucceeds_PersistsProperties()
        {
            // Arrange
            PropertyIngestResult result = new (Run: DateTime.Now, Success: true, ValidProperties: [], InvalidProperties: []);

            mockPropertyIngestService.Setup(x => x.IngestPropertiesAsync())
                .ReturnsAsync(result);

            // Act
            Func<Task> action = () => this.service.RunIngestPropertiesJobAsync();

            // Assert
            await action.Should().NotThrowAsync();
            mockPropertyIngestService.Verify(x => x.IngestPropertiesAsync(), Times.Once);
            mockPropertiesRepository.Verify(x => x.UpsertPropertiesAsync(result.Run, result.ValidProperties), Times.Once);
        }
    }
}
