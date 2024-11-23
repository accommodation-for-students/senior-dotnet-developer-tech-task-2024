using AFS.TechTask.Properties.Properties;

namespace AFS.TechTask.Properties
{
    /// <summary>
    /// Repository for <see cref="Property"/> entities.
    /// </summary>
    public interface IPropertiesRepository
    {
        /// <summary>
        /// Insert or update a collection of ingested properties.
        /// </summary>
        /// <param name="ingested">The timestamp for when the given properties were ingested.</param>
        /// <param name="properties">The properties to add or update.</param>
        Task UpsertPropertiesAsync(DateTime ingested, IReadOnlyCollection<Property> properties);

        /// <summary>
        /// Retrieve a <see cref="Property"/> with the given Id.
        /// </summary>
        /// <param name="propertyId">The Id of the property to retrieve.</param>
        /// <returns>The <see cref="Property"/> with the given Id.</returns>
        Task<Property> GetPropertyByIdAsync(int propertyId);

        /// <summary>
        /// Retrieve a collection of all <see cref="Property"/>.
        /// </summary>
        /// <returns>A collection of all <see cref="Property"/>.</returns>
        Task<IReadOnlyCollection<Property>> GetAllPropertiesAsync();
    }
}
