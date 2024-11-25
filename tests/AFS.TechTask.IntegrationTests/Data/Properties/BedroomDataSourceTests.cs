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
    public class BedroomsDataSourceTests
    {
        private readonly BedroomsDataSource dataSource;
        private readonly PropertiesDataSource properties;

        public BedroomsDataSourceTests()
        {
            var connectionFactory = new DbConnectionFactory(Options.Create(new ConnectionStringOptions()));
            this.properties = new PropertiesDataSource(connectionFactory);
            this.dataSource = new BedroomsDataSource(connectionFactory);
        }

        [Fact]
        public async Task InsertAndGetPhotosByPropertyId_WorksAsExpected()
        {
            // Arrange
            PropertyDataModel property = new()
            {
                PropertyType = (int)PropertyTypes.Flat,
                Country = ROI.Name,
                IngestRunId = 12.September(2024)
            };

            int propertyId = await this.properties.InsertPropertyAsync(property);

            BedroomDataModel[] bedrooms = 
            [
                new () { PropertyId = propertyId, Available = true, RoomSize = LargeRoom, BedSize = DoubleBed, Rent = 375, Deposit = 500 },
                new () { PropertyId = propertyId, Available = false, RoomSize = LargeRoom, BedSize = DoubleBed, Rent = 575, Deposit = 800 },
                new () { PropertyId = propertyId, Available = true, RoomSize = LargeRoom, BedSize = DoubleBed, Rent = 355, Deposit = 400 }
            ];

            // Act
            await this.dataSource.InsertBedroomsAsync(bedrooms);

            // Assert
            ICollection<BedroomDataModel> results = await this.dataSource.GetBedroomsByPropertyIdAsync(propertyId);
            results.Should().BeEquivalentTo(bedrooms, o => o.Excluding(p => p.BedroomId));
            results.All(x => x.BedroomId > 0).Should().BeTrue();
        }

        [Fact]
        public async Task GetBedroomsByPropertyIdAsync_UnknownPropertyId_ReturnsEmptyCollection()
        {
            // Act
            ICollection<BedroomDataModel> results = await this.dataSource.GetBedroomsByPropertyIdAsync(int.MaxValue);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task InsertBedroomsAsync_UnknownPropertyId_ThrowsException()
        {
            // Arrange
            BedroomDataModel[] bedrooms = [new() { PropertyId = int.MaxValue, Available = true, RoomSize = LargeRoom, BedSize = DoubleBed, Rent = 375, Deposit = 500 }];

            // Act
            Func<Task> action = () => this.dataSource.InsertBedroomsAsync(bedrooms);

            // Assert
            (await action.Should().ThrowAsync<SqliteException>())
                .Which.Message.Should().Contain("FOREIGN KEY");
        }
    }
}