using AFS.TechTask.Data.Properties;
using AFS.TechTask.Data.Properties.Photos;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

using static AFS.TechTask.IntegrationTests.Data.Properties.PropertyTestConstants;

namespace AFS.TechTask.IntegrationTests.Data.Properties
{
    public class PhotosDataSourceTests
    {
        private readonly PhotosDataSource dataSource;
        private readonly PropertiesDataSource properties;

        public PhotosDataSourceTests()
        {
            var connectionFactory = new DbConnectionFactory(Options.Create(new ConnectionStringOptions()));
            this.properties = new PropertiesDataSource(connectionFactory);
            this.dataSource = new PhotosDataSource(connectionFactory);
        }

        [Fact]
        public async Task InsertAndGetPhotosByPropertyId_WorksAsExpected()
        {
            // Arrange
            PropertyDataModel property = new()
            {
                PropertyType = (int)PropertyTypes.House,
                Country = UK.Name,
                IngestRunId = 24.November(2024)
            };

            int propertyId = await this.properties.InsertPropertyAsync(property);

            PhotoDataModel[] photos = 
            [
                new () { PropertyId = propertyId, Uri = "http://aws.images/10" },
                new () { PropertyId = propertyId, Uri = "http://aws.images/11" },
                new () { PropertyId = propertyId, Uri = "http://aws.images/12" }
            ];

            // Act
            await this.dataSource.InsertPhotosAsync(photos);

            // Assert
            ICollection<PhotoDataModel> results = await this.dataSource.GetPhotosByPropertyIdAsync(propertyId);
            results.Should().BeEquivalentTo(photos, o => o.Excluding(p => p.PhotoId));
            results.All(x => x.PhotoId > 0).Should().BeTrue();
        }

        [Fact]
        public async Task GetPhotosByPropertyIdAsync_UnknownPropertyId_ReturnsEmptyCollection()
        {
            // Act
            ICollection<PhotoDataModel> results = await this.dataSource.GetPhotosByPropertyIdAsync(int.MaxValue);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task InsertPhotosAsync_UnknownPropertyId_ThrowsException()
        {
            // Arrange
            PhotoDataModel[] photos = [ new () { PropertyId = int.MaxValue, Uri = "http://aws.images/10" } ];

            // Act
            Func<Task> action = () => this.dataSource.InsertPhotosAsync(photos);

            // Assert
            (await action.Should().ThrowAsync<SqliteException>())
                .Which.Message.Should().Contain("FOREIGN KEY");
        }

        [Fact]
        public async Task InsertPhotosAsync_UniqueUriConstraint_ThrowsException()
        {
            // Arrange
            PropertyDataModel property = new()
            {
                PropertyType = (int)PropertyTypes.House,
                Country = UK.Name,
                IngestRunId = 24.November(2024)
            };

            int propertyId = await this.properties.InsertPropertyAsync(property);

            PhotoDataModel[] photos =
            [
                new () { PropertyId = propertyId, Uri = "http://aws.images/10" },
                new () { PropertyId = propertyId, Uri = "http://aws.images/10" }
            ];

            // Act
            Func<Task> action = () => this.dataSource.InsertPhotosAsync(photos);

            // Assert
            (await action.Should().ThrowAsync<SqliteException>())
                .Which.Message.Should().Contain("UNIQUE");
        }
    }
}