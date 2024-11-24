namespace AFS.TechTask.Application.Properties
{
    /// <summary>
    /// Application service for interacting with properties.
    /// </summary>
    public interface IPropertyService
    {
        /// <summary>
        /// Ingest properties from an external source and persist if valid.
        /// </summary>
        Task RunIngestPropertiesJobAsync();
    }
}
