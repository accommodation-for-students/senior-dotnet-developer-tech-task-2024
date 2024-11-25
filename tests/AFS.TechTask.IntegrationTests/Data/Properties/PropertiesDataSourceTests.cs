using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Options;

using static AFS.TechTask.IntegrationTests.Data.Properties.PropertyTestConstants;

namespace AFS.TechTask.IntegrationTests.Data.Properties
{
    public class PropertiesDataSourceTests
    {
        private readonly PropertiesDataSource dataSource;

        public PropertiesDataSourceTests()
        {
            var connectionFactory = new DbConnectionFactory(Options.Create(new ConnectionStringOptions()));
            this.dataSource = new PropertiesDataSource(connectionFactory);
        }

        [Fact]
        public async Task InsertAndGetProperty_WorksAsExpected()
        {
            // Arrange
            PropertyDataModel property = new()
            {
                PropertyType = (int)PropertyTypes.House,
                Country = UK.Name,
                IngestRunId = 24.November(2024)
            };

            // Act
            int propertyId = await this.dataSource.InsertPropertyAsync(property);

            // Assert
            PropertyDataModel result = await this.dataSource.GetPropertyByIdAsync(propertyId);
            result.Should().BeEquivalentTo(property, o => o.Excluding(p => p.Id));
            result.Id.Should().NotBe(property.Id);
        }
    }
}