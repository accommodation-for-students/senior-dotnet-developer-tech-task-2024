using System.Data;

namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Datasource for the Bedroom table
    /// </summary>
    public interface IBedroomsDataSource
    {
        /// <summary>
        /// Insert a <see cref="BedroomDataModel"/>.
        /// </summary>
        /// <param name="bedroom">The bedroom to insert</param>
        /// <param name="transaction">Optional transaction to execute within.</param>
        Task InsertBedroomsAsync(ICollection<BedroomDataModel> bedroom, IDbTransaction transaction = null);

        /// <summary>
        /// Retrieve all <see cref="BedroomDataModel"/>s associated with a property.
        /// </summary>
        /// <param name="propertyId">The Id of the property the bedrooms belong to.</param>
        Task<ICollection<BedroomDataModel>> GetBedroomsByPropertyIdAsync(int propertyId);
    }
}
