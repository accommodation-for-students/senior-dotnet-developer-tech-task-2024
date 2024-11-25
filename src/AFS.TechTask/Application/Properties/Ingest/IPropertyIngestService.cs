namespace AFS.TechTask.Application.Properties.Ingest
{
    /// <summary>
    /// Service for ingesting <see cref="PropertyResponse"/> from an external source.
    /// </summary>
    public interface IPropertyIngestService
    {
        /// <summary>
        /// Retrieve and validate property records from an external source.
        /// </summary>
        /// <returns>A <see cref="PropertyIngestService"/> containing validated and invalid property records.</returns>
        Task<PropertyIngestResult> IngestPropertiesAsync();
    }
}
