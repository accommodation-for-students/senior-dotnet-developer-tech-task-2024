using AFS.TechTask.Domain.Properties;

namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Repository for <see cref="Property"/> entities.
    /// </summary>
    public interface IPropertiesRepository
    {
        /// <summary>
        /// Insert a <see cref="Property"/>.
        /// </summary>
        /// <param name="ingestRunIdentifier">The timestamp for when the given property was ingested.</param>
        /// <param name="property">The property to add.</param>
        /// <returns>The Id of the created property.</returns>
        Task<int> InsertPropertyAsync(DateTime ingestRunIdentifier, Property property);

        /// <summary>
        /// Retrieve a <see cref="Property"/> with the given Id.
        /// </summary>
        /// <param name="propertyId">The Id of the property to retrieve.</param>
        /// <returns>The <see cref="Property"/> with the given Id.</returns>
        Task<Property> GetPropertyByIdAsync(int propertyId);
    }
}
