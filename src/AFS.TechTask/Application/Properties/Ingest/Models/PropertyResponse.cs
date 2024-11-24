namespace AFS.TechTask.Application.Properties.Ingest
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
