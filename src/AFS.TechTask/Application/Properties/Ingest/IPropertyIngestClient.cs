namespace AFS.TechTask.Application.Properties.Ingest
{
    /// <summary>
    /// Client for fetching <see cref="PropertyResponse"/> records from an external source.
    /// </summary>
    public interface IPropertyIngestClient
    {
        /// <summary>
        /// Retrieve <see cref="PropertyResponse"/> records from an external source.
        /// </summary>
        /// <returns>A colelction of <see cref="PropertyResponse"/> records.</returns>
        Task<IReadOnlyCollection<PropertyResponse>> GetPropertiesAsync();
    }
}
