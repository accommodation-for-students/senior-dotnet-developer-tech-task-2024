namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Datasource for the Property table
    /// </summary>
    public interface IPropertiesDataSource
    {
        /// <summary>
        /// Insert or update a <see cref="PropertyDataModel"/>.
        /// </summary>
        /// <param name="property">The property to upsert</param>
        /// <returns>The Id of the created property.</returns>
        Task<int> InsertPropertyAsync(PropertyDataModel property);

        /// <summary>
        /// Retrieve a <see cref="PropertyDataModel"/> with the given Id.
        /// </summary>
        Task<PropertyDataModel> GetPropertyByIdAsync(int propertyId);

        /// <summary>
        /// Retrieve a collection of all <see cref="PropertyDataModel"/>s.
        /// </summary>
        Task<ICollection<PropertyDataModel>> GetAllPropertiesAsync();
    }
}
