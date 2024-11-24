namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Represents a property in the database.
    /// </summary>
    public class PropertyDataModel
    {
        public int Id { get; set; }

        public int PropertyType { get; set; }

        public string Country { get; set; }

        public DateTime IngestRunId { get; set; }
    }
}
