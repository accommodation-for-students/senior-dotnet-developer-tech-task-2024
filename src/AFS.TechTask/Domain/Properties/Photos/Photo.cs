namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a URI to a photograph of a property.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// Id of the photo, once persisted.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The Id of the property the photo is of, once persisted.
        /// </summary>
        public int PropertyId { get; init; }

        /// <summary>
        /// String representing a URI to a photograph of a property.
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Initalises a new instance of the <see cref="Photo"/> class.
        /// </summary>
        /// <param name="uri">A URI to an online photo as a string.</param>
        public Photo(string uri)
        {
            // Validate that the URI string is correctly constructed, throws if invalid
            Uri _ = new Uri(uri);
            
            //TODO: consider security concerns when ingesting URLs from external sources

            this.Uri = uri;
        }

        /// <summary>
        /// String representation of this photo's URI.
        /// </summary>
        /// <returns>This photo's URI as a string.</returns>
        public override string ToString() => this.Uri;
    }
}
