using AFS.TechTask.Infrastructure;
using Dapper;
using System.Data;

namespace AFS.TechTask.Data.Properties.Photos
{
    /// <summary>
    /// Datasource for the Bedroom table
    /// </summary>
    public class BedroomsDataSource : IBedroomsDataSource
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        /// <summary>
        ///  Instantiates an instance of the <see cref="BedroomsDataSource"/> class.
        /// </summary>
        public BedroomsDataSource(IDbConnectionFactory sqlConnectionFactory)
        {
            this.dbConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Insert a set of <see cref="BedroomDataModel"/>s for a property.
        /// </summary>
        /// <param name="bedrooms">The bedrooms to insert.</param>
        /// <param name="transaction">Optional transaction to execute within.</param>
        public async Task InsertBedroomsAsync(ICollection<BedroomDataModel> bedrooms, IDbTransaction transaction = null)
        {
            const string sql = @"INSERT INTO Bedroom (PropertyId, Available, RoomSize, BedSize, Rent, Deposit) 
                                 VALUES (@PropertyId, @Available, @RoomSize, @BedSize, @Rent, @Deposit);";

            if (transaction != null)
            {
                await transaction.Connection.ExecuteAsync(sql, bedrooms, transaction);
            }
            else
            {
                using (IDbConnection connection = await this.dbConnectionFactory.CreateConnectionAsync())
                {
                    await connection.ExecuteAsync(sql, bedrooms);
                }
            }
        }

        /// <summary>
        /// Retrieve all <see cref="BedroomDataModel"/>s associated with a property.
        /// </summary>
        /// <param name="propertyId">The Id of the property the bedrooms are in.</param>
        public async Task<ICollection<BedroomDataModel>> GetBedroomsByPropertyIdAsync(int propertyId)
        {
            const string sql = @"SELECT BedroomId, PropertyId, Available, RoomSize, BedSize, Rent, Deposit
                                 FROM Bedroom 
                                 WHERE PropertyId = @PropertyId;";

            using (IDbConnection connection = await this.dbConnectionFactory.CreateConnectionAsync())
            {
                IEnumerable<BedroomDataModel> results = await connection.QueryAsync<BedroomDataModel>(sql, new { PropertyId = propertyId });
                return results.ToArray();
            }
        }
    }
}
