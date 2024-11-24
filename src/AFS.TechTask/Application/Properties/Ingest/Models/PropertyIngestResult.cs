using AFS.TechTask.Domain.Properties;

namespace AFS.TechTask.Application.Properties.Ingest
{
    /// <summary>
    /// Result of a property ingest operation.
    /// </summary>
    /// <param name="Run">Identifier for when an ingest operation was run.</param>
    /// <param name="Success">Whether the operation succeeded.</param>
    /// <param name="ValidProperties">Successfully validated properties.</param>
    /// <param name="InvalidProperties">Invalid ingested properties.</param>
    public record PropertyIngestResult(
        DateTime Run, 
        bool Success, 
        IReadOnlyCollection<Property> ValidProperties, 
        IReadOnlyCollection<InvalidPropertyIngest> InvalidProperties)
    {
        /// <summary>
        /// Returns an empty and invalid <see cref="PropertyIngestResult"/>
        /// </summary>
        /// <param name="Run">Identifier for when the result was generated.</param>
        public static PropertyIngestResult InvalidResult(DateTime run) => new PropertyIngestResult(run, false, Array.Empty<Property>(), Array.Empty<InvalidPropertyIngest>());
    }
}
