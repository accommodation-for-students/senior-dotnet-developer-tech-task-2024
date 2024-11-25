using AFS.TechTask.Infrastructure;
using Dapper;
using System.Data;

namespace AFS.TechTask.Data.Properties.Photos
{
    /// <summary>
    /// Datasource for the Photo table
    /// </summary>
    public class PhotosDataSource : IPhotosDataSource
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        /// <summary>
        ///  Instantiates an instance of the <see cref="PhotosDataSource"/> class.
        /// </summary>
        public PhotosDataSource(IDbConnectionFactory sqlConnectionFactory)
        {
            this.dbConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Insert a set of <see cref="PhotoDataModel"/>s for a property.
        /// </summary>
        /// <param name="photos">The photos to insert.</param>
        /// <param name="transaction">Optional transaction to execute within.</param>
        public async Task InsertPhotosAsync(ICollection<PhotoDataModel> photos, IDbTransaction transaction = null)
        {
            const string sql = "INSERT INTO Photo (PropertyId, Uri) VALUES (@PropertyId, @Uri);";

            if (transaction != null)
            {
                await transaction.Connection.ExecuteAsync(sql, photos, transaction);
            }
            else
            {
                using (IDbConnection connection = await this.dbConnectionFactory.CreateConnectionAsync())
                {
                    await connection.ExecuteAsync(sql, photos);
                }
            }
        }

        /// <summary>
        /// Retrieve all <see cref="PhotoDataModel"/>s associated with a property.
        /// </summary>
        /// <param name="propertyId">The Id of the property the photos belong to.</param>
        public async Task<ICollection<PhotoDataModel>> GetPhotosByPropertyIdAsync(int propertyId)
        {
            const string sql = @"SELECT PhotoId, PropertyId, Uri
                                 FROM Photo 
                                 WHERE PropertyId = @PropertyId;";

            using (IDbConnection connection = await this.dbConnectionFactory.CreateConnectionAsync())
            {
                IEnumerable<PhotoDataModel> results = await connection.QueryAsync<PhotoDataModel>(sql, new { PropertyId = propertyId });
                return results.ToArray();
            }
        }
    }
}
