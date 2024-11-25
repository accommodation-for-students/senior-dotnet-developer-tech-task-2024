namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Represents a property photo in the database.
    /// </summary>
    public class PhotoDataModel
    {
        public int PhotoId { get; set; }

        public int PropertyId { get; set; }

        public string Uri { get; set; }
    }
}
