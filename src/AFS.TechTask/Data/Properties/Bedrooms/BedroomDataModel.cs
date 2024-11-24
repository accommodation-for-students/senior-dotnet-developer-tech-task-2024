namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Represents a bedroom in the database.
    /// </summary>
    public class BedroomDataModel
    {
        public int BedroomId { get; set; }

        public int PropertyId { get; set; }

        public bool Available { get; set; }

        public string RoomSize { get; set; }

        public string BedSize { get; set; }

        public int Rent { get; set; }

        public int Deposit { get; set; }
    }
}
