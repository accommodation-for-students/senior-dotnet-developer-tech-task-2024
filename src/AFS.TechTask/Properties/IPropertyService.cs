namespace AFS.TechTask.Properties
{
    /// <summary>
    /// Service for interacting with <see cref="Property"/> records
    /// </summary>
    public interface IPropertyService
    {
        /// <summary>
        /// Ingest properties from an external source and persist if valid.
        /// </summary>
        Task RunIngestPropertiesJobAsync();
    }
}
