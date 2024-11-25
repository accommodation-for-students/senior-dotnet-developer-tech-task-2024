namespace AFS.TechTask.Application.Properties.Ingest
{
    /// <summary>
    /// Configurable property ingest options.
    /// </summary>
    public class PropertyIngestOptions
    {
        /// <summary>
        /// The base URL for the external source to retrieve properties from.
        /// </summary>
        public string BaseURL { get; set; } = "https://www.studentproperties.com";
    }
}
