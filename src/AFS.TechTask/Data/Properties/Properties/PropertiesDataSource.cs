using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using Dapper;
using System.Data;

namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Datasource for <see cref="PropertyDataModel"/> entities.
    /// </summary>
    public class PropertiesDataSource : IPropertiesDataSource
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        /// <summary>
        ///  Instantiates an instance of the <see cref="PropertiesRepository"/> class.
        /// </summary>
        public PropertiesDataSource(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// Insert a <see cref="PropertyDataModel"/>.
        /// </summary>
        /// <param name="property">The property to insert</param>
        /// <param name="transaction">Optional transaction to execute within.</param>
        /// <returns>The Id of the created property.</returns>
        public async Task<int> InsertPropertyAsync(PropertyDataModel property, IDbTransaction transaction = null)
        {
            const string sql = @"INSERT INTO Property (PropertyType, Country, IngestRunId)
                                 VALUES (@PropertyType, @Country, @IngestRunId)";

            IDbConnection connection = transaction?.Connection ?? await this.dbConnectionFactory.CreateConnectionAsync();

            if (transaction != null)
            {
                await transaction.Connection.ExecuteAsync(sql, property, transaction);
            }
            else
            {
                using (connection)
                {
                    await connection.ExecuteAsync(sql, property);
                }
            }

            int propertyId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ROWID();");
            return propertyId;
        }

        /// <summary>
        /// Retrieve a <see cref="Property"/> with the given Id.
        /// </summary>
        /// <param name="propertyId">The Id of the property to retrieve.</param>
        /// <returns>The <see cref="Property"/> with the given Id.</returns>
        public async Task<PropertyDataModel> GetPropertyByIdAsync(int propertyId)
        {
            const string sql = @"SELECT Id, PropertyType, Country, IngestRunId
                                FROM Property 
                                WHERE Id = @PropertyId;";

            using (IDbConnection connection = await this.dbConnectionFactory.CreateConnectionAsync())
            {
                PropertyDataModel result = await connection.QueryFirstOrDefaultAsync<PropertyDataModel>(sql, new { PropertyId = propertyId });
                return result ?? throw new KeyNotFoundException($"Property Id {propertyId} not found.");
            }
        }

        /// <summary>
        /// Retrieve a collection of all <see cref="Property"/>.
        /// </summary>
        /// <returns>A collection of all <see cref="Property"/>.</returns>
        public async Task<ICollection<PropertyDataModel>> GetAllPropertiesAsync()
        {
            const string sql = "SELECT Id, PropertyType, Country, IngestRunId FROM Property;";

            using (IDbConnection connection = await this.dbConnectionFactory.CreateConnectionAsync())
            {
                IEnumerable<PropertyDataModel> result = await connection.QueryAsync<PropertyDataModel>(sql);
                return result?.ToArray() ?? [];
            }
        }
    }
}
