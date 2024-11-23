namespace AFS.TechTask.Properties.Ingest.Models
{
    /// <summary>
    /// A property record ingested from an external source.
    /// </summary>
    public record PropertyResponse(
        string PropertyType,
        ICollection<BedroomResponse> Bedrooms,
        ICollection<string> Photos,
        string Country)
    {
    }
}
