namespace AFS.TechTask.Properties.Ingest.Models
{
    /// <summary>
    /// A bedroom record ingested from an external source.
    /// </summary>
    public record BedroomResponse(
        bool Available,
        string RoomSize,
        string BedSize,
        int Rent,
        int Deposit);
}
