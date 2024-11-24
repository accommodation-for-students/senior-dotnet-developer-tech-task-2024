namespace AFS.TechTask.Application.Properties.Ingest
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
